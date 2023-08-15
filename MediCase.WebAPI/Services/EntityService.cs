using AutoMapper;
using MediCase.WebAPI.Models.Entity;
using MediCase.WebAPI.Repositories.Content.Interfaces;
using MediCase.WebAPI.Services.Interfaces;

namespace MediCase.WebAPI.Services
{
    public class EntityService : IEntityService
    {
        private readonly IEntityTypeRepository _entityTypeRepository;
        private readonly IEntityLanguageRepository _entityLanguageRepository;
        private readonly IEntityRepository _entityRepository;
        private readonly IEntityGraphDataRepository _entityGraphDataRepository;
        private readonly IEntityTranslationsRepository _entityTranslationsRepository;
        private readonly IEntityTranslationFilesRepository _entityTranslationFilesRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EntityService(IEntityTypeRepository entityTypeRepository,
            IEntityLanguageRepository entityLanguageRepository,
            IEntityRepository entityRepository,
            IEntityGraphDataRepository entityGraphDataRepository,
            IEntityTranslationsRepository entityTranslationsRepository,
            IEntityTranslationFilesRepository entityTranslationFilesRepository,
            IFileService fileService,
            ILogger<EntityService> logger,
            IMapper mapper)
        {
            _entityTypeRepository = entityTypeRepository;
            _entityLanguageRepository = entityLanguageRepository;
            _entityRepository = entityRepository;
            _entityGraphDataRepository = entityGraphDataRepository;
            _entityTranslationsRepository = entityTranslationsRepository;
            _entityTranslationFilesRepository = entityTranslationFilesRepository;
            _fileService = fileService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<EntityTypeDto>> GetEntityTypesAsync()
        {
            var entityType = await _entityTypeRepository.GetEntityTypesAsync();
            return _mapper.Map<List<EntityTypeDto>>(entityType);
        }
        public async Task<List<EntityLanguageGetDto>> GetLanguagesAsync()
        {
            var entityType = await _entityLanguageRepository.GetLanguagesAsync();
            return _mapper.Map<List<EntityLanguageGetDto>>(entityType);
        }

        public async Task<List<EntityDto>> GetEntitiesAsync()
        {
            return _mapper.Map<List<EntityDto>>(await _entityRepository.GetEntitiesAsync());
        }
        public async Task<EntityWithTranslationDto> GetEntityWithTranslationsAsync(uint entityId)
        {
            return _mapper.Map<EntityWithTranslationDto>(await _entityRepository.GetEntityWithTranslationsAsync(entityId));
        }
        public async Task<List<EntityWithTranslationDto>> GetEntitiesWithTranslationsAsync()
        {
            return _mapper.Map<List<EntityWithTranslationDto>>(await _entityRepository.GetEntitiesWithTranslationsAsync());
        }

        public async Task<EntityGraphObjectDto> GetEntityWithChildsAsync(uint? entityId)
        {
            return _mapper.Map<EntityGraphObjectDto>(await _entityGraphDataRepository.GetEntityWithChildsAsync(entityId));
        }
        public async Task<EntityGraphObjectDto> GetEntityWithChildsByLanguageAsync(uint? entityId, uint[] langIDs)
        {
            return _mapper.Map<EntityGraphObjectDto>(await _entityGraphDataRepository.GetEntityWithChildsByLanguageAsync(entityId, langIDs));
        }
        public async Task<List<EntityWithTranslationDto>> GetEntityChildsByLanguageAsync(uint? entityId, uint[] langIDs)
        {
            var entityList = await _entityGraphDataRepository.GetEntityChildsByLanguageAsync(entityId, langIDs);
            if (!entityList.Any()) throw new InvalidOperationException();
            return _mapper.Map<List<EntityWithTranslationDto>>(entityList);
        }
        public async Task<List<EntityTranslationDto>> GetEntityTranslationsAsync()
        {
            return _mapper.Map<List<EntityTranslationDto>>(await _entityTranslationsRepository.GetEntityTranslationsAsync());
        }
        public async Task<Stream> GetEntityTranslationFileAsync(string filePath)
        {
            string actualFileName = await _entityTranslationFilesRepository.GetActualFilePathAsync(filePath);
            return await _fileService.GetFileStreamAsync(actualFileName);
        }
    }
}
