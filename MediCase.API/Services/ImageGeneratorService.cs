using AutoMapper;
using MediCase.API.Entities.Moderator;
using MediCase.API.Models.Entity.Moderator;
using MediCase.API.Repositories.Moderator.Interfaces;
using MediCase.API.Services.Interfaces;
using MediCase.API.Repositories.Moderator;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Images;
using System.Net;

namespace MediCase.API.Services
{
    public class ImageGeneratorService : IImageGeneratorService
    {
        private readonly IEntityTranslationFilesRepository _entityTranslationFilesRepository;
        private readonly IFileService _fileService;
        private readonly IEntityTranslationsRepository _entityTranslationsRepository;
        private readonly IModeratorQueryBucketRepository _moderatorQueryBucketRepository;
        private readonly IMediCaseModTransactionRepository _moderatorTransactionRepository;
        private readonly JsonSerializerSettings serializerSettings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly OpenAIAPI api;
        private readonly IConfiguration _config;
        public ImageGeneratorService(IEntityTranslationFilesRepository entityTranslationFilesRepository,
            IEntityTranslationsRepository entityTranslationRepository,
            IModeratorQueryBucketRepository moderatorQueryBucketRepository,
            IMediCaseModTransactionRepository moderatorTransactionRepository,
            IFileService fileService,
            ILogger<ImageGeneratorService> logger,
            IConfiguration configuration,
            IMapper mapper)
        {
            _entityTranslationFilesRepository = entityTranslationFilesRepository;
            _fileService = fileService;
            _entityTranslationsRepository = entityTranslationRepository;
            _moderatorQueryBucketRepository = moderatorQueryBucketRepository;
            _moderatorTransactionRepository = moderatorTransactionRepository;
            _logger = logger;
            _config = configuration;
            _mapper = mapper;
            api = new OpenAIAPI(_config["ApiKeys:OpenAI"]);
            serializerSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        }

        public async Task GenerateTranslationFileAsync(uint translationId, string? customPrompt)
        {
            ImageGenerationRequest generationRequest = new ImageGenerationRequest();
            ImageResult resultImage;

            if (customPrompt != null)
            {
                generationRequest.Prompt = customPrompt;
                generationRequest.ResponseFormat = ImageResponseFormat.Url;
                generationRequest.Size = ImageSize._256;
                resultImage = await api.ImageGenerations.CreateImageAsync(generationRequest);
            }
            else
            {
                generationRequest.Prompt = await _entityTranslationsRepository.GetEntityTranslationMainTitleAsync(translationId);
                generationRequest.ResponseFormat = ImageResponseFormat.Url;
                generationRequest.Size = ImageSize._256;
                if (generationRequest.Prompt == null) throw new ArgumentException();
                resultImage = await api.ImageGenerations.CreateImageAsync(customPrompt + " simple icon image");
            }
            byte[] bytes;
            using (HttpClient client = new HttpClient())
            {
                bytes = await client.GetByteArrayAsync(resultImage.Data[0].Url);
            }

            MemoryStream memoryStream = new MemoryStream(bytes);
            IFormFile resultFile = new FormFile(memoryStream, 0, bytes.Length, "something.png", "something.png");

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
    }
}
