using AutoMapper;
using MediCase.WebAPI.Services.Interfaces;
using MediCase.WebAPI.Repositories.Moderator.Interfaces;
using MediCase.WebAPI.Models.Entity;
using OpenAI_API;
using MediCase.WebAPI.Exceptions;


namespace MediCase.WebAPI.Services
{
    public class TranslationGeneratorService : ITranslationGeneratorService
    {
        private readonly IEntityTranslationsRepository _entityTranslationsRepository;
        private readonly IEntityLanguageRepository _entityLanguageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly OpenAIAPI api;
        private readonly IConfiguration _config;
        private readonly OpenAI_API.Chat.Conversation translator;
        private readonly string promptSchema;
        

        public TranslationGeneratorService(IEntityTranslationsRepository entityTranslationsRepository,
            IEntityLanguageRepository entityLanguageRepository,
            ILogger<TranslationGeneratorService> logger,
            IConfiguration configuration,
            IMapper mapper)
        {
            _entityTranslationsRepository = entityTranslationsRepository;
            _entityLanguageRepository = entityLanguageRepository;
            _mapper = mapper;
            _logger = logger;
            _config = configuration;
            promptSchema = "Translate this from [SOURCE_LANGUAGE] string to [DESIRED_LANGUAGE], keeping the html markups: [CONTENT]";
            api = new OpenAIAPI(_config["ApiKeys:OpenAI"]);
            translator = api.Chat.CreateConversation();
            translator.AppendSystemMessage("You are translator, you will be translating text given by user. User will ask his request like this: Translate this from [SOURCE_LANGUAGE] to [DESIRED_LANGUAGE], keep the html markups: [CONTENT]. You need translate everything provided in [CONTENT] from [SOURCE_LANGUAGE] to [DESIRED_LANGUAGE]. Keep all html marsk given in [CONTENT]");
            translator.AppendUserInput("Translate this from PL string to US, keeping the html markups: <p>Cześć!</p>");
            translator.AppendExampleChatbotOutput("<p>Hello!</p>");
            translator.AppendUserInput("Translate this from PL string to US, keeping the html markups: <br>Cześć!<br>");
            translator.AppendExampleChatbotOutput("<br><Hallo!<br>");
            translator.AppendUserInput("Translate this from PL string to CZ, keeping the html markups: <ul>Mam na imię Kasia!</ul>");
            translator.AppendExampleChatbotOutput("<ul>Jmenuji se Kasia</ul>");
        }

        public async Task<EntityTranslationDto> GenerateTranslationAsync(uint translationId, uint desiredLanguageId)
        {
                EntityTranslationDto desiredTranslation = new EntityTranslationDto();
                desiredTranslation.LangId = desiredLanguageId;
                var sourceTranslation = await _entityTranslationsRepository.GetEntityTranslationAsync(translationId);
                desiredTranslation.EntityId = sourceTranslation.EntityId;
                var sourceLanguage = sourceTranslation.Lang.LangValue;
                var desiredLanguage = await _entityLanguageRepository.GetLangValueAsync(desiredLanguageId);
                if (sourceTranslation.MainTitle == "" || sourceTranslation.MainTitle == null) desiredTranslation.MainTitle = "";
                else
                {
                    translator.AppendUserInput(promptSchema.Replace("[SOURCE_LANGUAGE]", sourceLanguage).Replace("[DESIRED_LANGUAGE]", desiredLanguage).Replace("[CONTENT]", sourceTranslation.MainTitle));
                    desiredTranslation.MainTitle = await translator.GetResponseFromChatbotAsync();
                    desiredTranslation.MainTitle = desiredTranslation.MainTitle;
                }

                if (sourceTranslation.SubTitle == "" || sourceTranslation.SubTitle == null) desiredTranslation.SubTitle = "";
                else
                {
                    translator.AppendUserInput(promptSchema.Replace("[SOURCE_LANGUAGE]", sourceLanguage).Replace("[DESIRED_LANGUAGE]", desiredLanguage).Replace("[CONTENT]", sourceTranslation.SubTitle));
                    desiredTranslation.SubTitle = await translator.GetResponseFromChatbotAsync();
                    desiredTranslation.SubTitle = desiredTranslation.SubTitle;
                }

                if (sourceTranslation.Paragrahps == "" || sourceTranslation.Paragrahps == null) desiredTranslation.Paragrahps = "";
                else
                {
                    translator.AppendUserInput(promptSchema.Replace("[SOURCE_LANGUAGE]", sourceLanguage).Replace("[DESIRED_LANGUAGE]", desiredLanguage).Replace("[CONTENT]", sourceTranslation.Paragrahps));
                    desiredTranslation.Paragrahps = await translator.GetResponseFromChatbotAsync();
                    desiredTranslation.Paragrahps = desiredTranslation.Paragrahps;

                }
            return desiredTranslation;
        }
    }
}
