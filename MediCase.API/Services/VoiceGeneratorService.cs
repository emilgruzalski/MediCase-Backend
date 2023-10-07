using Amazon.Polly;
using Amazon.Polly.Model;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Diagnostics.Metrics;
using AutoMapper;
using MediCase.API.Exceptions;
using MediCase.API.Services.Interfaces;
using MediCase.API.Repositories.Moderator.Interfaces;
using MediCase.API.Models.Entity.Moderator;
using MediCase.API.Entities.Moderator;

namespace MediCase.API.Services
{
    public class VoiceGeneratorService : IVoiceGeneratorService
    {
        private readonly IEntityTranslationsRepository _entityTranslationsRepository;
        private readonly IEntityTranslationFilesRepository _entityTranslationFilesRepository;
        private readonly IModeratorQueryBucketRepository _moderatorQueryBucketRepository;
        private readonly IMediCaseModTransactionRepository _moderatorTransactionRepository;
        private readonly IFileService _fileService;
        private readonly JsonSerializerSettings serializerSettings;
        private readonly AmazonPollyClient _pollyClient;
        private Dictionary<string, Tuple<string, string>> _avainableLanguages;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public VoiceGeneratorService(IEntityTranslationsRepository entityTranslationsRepository,
            IEntityTranslationFilesRepository entityTranslationFilesRepository,
            IModeratorQueryBucketRepository moderatorQueryBucketRepository,
            IMediCaseModTransactionRepository moderatorTransactionRepository,
            IConfiguration config,
            IFileService fileService,
            ILogger<VoiceGeneratorService> logger,
            IMapper mapper)
        {
            _entityTranslationsRepository = entityTranslationsRepository;
            _entityTranslationFilesRepository = entityTranslationFilesRepository;
            _moderatorQueryBucketRepository = moderatorQueryBucketRepository;
            _moderatorTransactionRepository = moderatorTransactionRepository;
            _fileService = fileService;
            _logger = logger;
            _config = config;
            _mapper = mapper;
            _pollyClient = new AmazonPollyClient(_config["ApiKeys:AmazonAccessKey"], _config["ApiKeys:AmazonSecretAccessKey"], Amazon.RegionEndpoint.EUCentral1);
            serializerSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            _avainableLanguages = new Dictionary<string, Tuple<string, string>>()
            {
                {"US", Tuple.Create("en-US", "Joanna")},
                {"PL", Tuple.Create("pl-PL", "Ola")},
                {"DE", Tuple.Create("de-DE", "Daniel")},
                {"FR", Tuple.Create("fr-FR", "Rémi")},
                {"ES", Tuple.Create("es-US", "Pedro")}
            };
        }

        public Task<List<string>> GetSupportedLanguagesAsync()
        {
            return Task.Factory.StartNew(() => _avainableLanguages.Keys.ToList());
        }
        public async Task GenerateVoiceAsync(uint translationId, string? refferedField)
        {
            EntityTranslation sourceTranslation = await _entityTranslationsRepository.GetEntityTranslationAsync(translationId);
            var voiceTuple = GetMatchingVoiceAsync(await _entityTranslationsRepository.GetEntityTranslationLanguageAsync(translationId));

            string? requestText;
            if (refferedField == null) requestText = sourceTranslation.MainTitle;
            else if (refferedField == "MainTitle") requestText = sourceTranslation.MainTitle;
            else if (refferedField == "SubTitle") requestText = sourceTranslation.SubTitle;
            else if (refferedField == "Paragraphs") requestText = sourceTranslation.Paragrahps;
            else requestText = sourceTranslation.MainTitle;

            requestText = Regex.Replace(requestText, @"(<li>|</li>|<ul>|</ul>)", "\n");
            requestText = Regex.Replace(requestText, @"<[^>]+>|&nbsp;", "\n").Trim();

            var speechRequest = new SynthesizeSpeechRequest()
            {
                OutputFormat = OutputFormat.Mp3,
                LanguageCode = voiceTuple.Item1,
                Text = requestText,
                VoiceId = voiceTuple.Item2,
                Engine = Engine.Neural
            };

            var synthesizeSpeechResponse = await _pollyClient.SynthesizeSpeechAsync(speechRequest);

            MemoryStream memoryStream = new MemoryStream();
            await synthesizeSpeechResponse.AudioStream.CopyToAsync(memoryStream);
            IFormFile resultFile = new FormFile(memoryStream, 0, memoryStream.Length, "something.mp3", "something.mp3");

            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {

                    var saveResult = await _fileService.SaveFileAsync(resultFile);
                    EntityTranslationFile entityTranslationFile = new EntityTranslationFile();
                    entityTranslationFile.FilePath = saveResult.Item1;
                    entityTranslationFile.TranslationId = translationId;
                    entityTranslationFile.FilePathHashed = saveResult.Item3;
                    entityTranslationFile.FilePriority = 0;
                    entityTranslationFile.FileType = saveResult.Item2;
                    entityTranslationFile.ReferredField = saveResult.Item2;
                    var returnedObject = await _entityTranslationFilesRepository.AddEntityTranslationFileAsync(entityTranslationFile);
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    newQueryBucketItem.OperationType = "INSERT";
                    newQueryBucketItem.DestinationTable = "EntityTranslationFiles";
                    newQueryBucketItem.QueryLog = JsonConvert.SerializeObject(_mapper.Map<ModeratorEntityTranslationFileDto>(returnedObject), serializerSettings);
                    await _moderatorQueryBucketRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                }
                catch (Exception ex)
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }

        private Tuple<string, string> GetMatchingVoiceAsync(string langCode)
        {
            foreach (var lang in _avainableLanguages)
            {
                if (lang.Key == langCode) return lang.Value;
            }
            throw new NotFoundException("This language is not supported");
        }
        /*
        private async void ProcessVoices()
        {
            var allVoicesRequest = new DescribeVoicesRequest();
            string nextToken;
            do
            {
                var allVoicesResponse = await _pollyClient.DescribeVoicesAsync(allVoicesRequest);
                nextToken = allVoicesResponse.NextToken;
                allVoicesRequest.NextToken = nextToken;
                var groupedVoices = allVoicesResponse.Voices.GroupBy(x => x.LanguageCode);
                foreach(var group in groupedVoices)
                {
                    var tuple = Tuple.Create(group.Key.ToString(), group.FirstOrDefault()?.Id.ToString());
                    _avainableLanguageCodes.Add(tuple);
                    Console.WriteLine(tuple);
                }

            }
            while (nextToken is not null);
        }
        */
    }
}
