using AutoMapper;
using MediCase.WebAPI.Entities.Moderator;
using MediCase.WebAPI.Models.Entity;
using Newtonsoft.Json;
using MediCase.WebAPI.Services.Interfaces;
using MediCase.WebAPI.Repositories.Moderator.Interfaces;
using MediCase.WebAPI.Models.Entity.Moderator;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
using MediCase.WebAPI.Exceptions;

namespace MediCase.WebAPI.Services
{
    public class ModEntityService : IModEntityService
    {
        private readonly IEntityTypeRepository _entityTypeRepository;
        private readonly IEntityLanguageRepository _entityLanguageRepository;
        private readonly IEntityRepository _entityRepository;
        private readonly IEntityGraphDataRepository _entityGraphDataRepository;
        private readonly IEntityTranslationsRepository _entityTranslationsRepository;
        private readonly IEntityTranslationFilesRepository _entityTranslationFilesRepository;
        private readonly IModeratorQueryBucketRepository _moderatorQueryBuckerRepository;
        private readonly IMediCaseModTransactionRepository _moderatorTransactionRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings serializerSettings;

        public ModEntityService(IEntityTypeRepository entityTypeRepository,
            IEntityLanguageRepository entityLanguageRepository,
            IEntityRepository entityRepository,
            IEntityGraphDataRepository entityGraphDataRepository,
            IEntityTranslationsRepository entityTranslationsRepository,
            IEntityTranslationFilesRepository entityTranslationFilesRepository,
            IModeratorQueryBucketRepository moderatorQueryBuckerRepository,
            IMediCaseModTransactionRepository moderatorTransactionRepository,
            IFileService fileService,
            ILogger<ModEntityService> logger,
            IMapper mapper)
        {
            _entityTypeRepository = entityTypeRepository;
            _entityLanguageRepository = entityLanguageRepository;
            _entityRepository = entityRepository;
            _entityGraphDataRepository = entityGraphDataRepository;
            _entityTranslationsRepository = entityTranslationsRepository;
            _entityTranslationFilesRepository = entityTranslationFilesRepository;
            _moderatorQueryBuckerRepository = moderatorQueryBuckerRepository;
            _moderatorTransactionRepository = moderatorTransactionRepository;
            _fileService = fileService;
            _mapper = mapper;
            _logger = logger;
            serializerSettings = new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore };
        }

        public async Task<List<EntityTypeDto>> GetEntityTypesAsync()
        {
            var entityType = await _entityTypeRepository.GetEntityTypesAsync();
            return _mapper.Map<List<EntityTypeDto>>(entityType);
        }
        public async Task UpdateEntityTypeAsync(EntityTypeDto entityType)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    var mappedObject = _mapper.Map<EntityType>(entityType);
                    newQueryBucketItem.OperationType = "UPDATE";
                    newQueryBucketItem.DestinationTable = "EntityTypes";
                    newQueryBucketItem.QueryLog = JsonConvert.SerializeObject(_mapper.Map<ModeratorEntityTypeDto>(mappedObject), serializerSettings);
                    await _entityTypeRepository.UpdateEntityTypeAsync(mappedObject);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                }
                catch (Exception ex) 
                { 
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<uint> AddEntityTypeAsync(string entityType)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    EntityType newEntityType = new EntityType();
                    newEntityType.TypeValue = entityType;
                    var returnedObject = await _entityTypeRepository.AddEntityTypeAsync(newEntityType);
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    newQueryBucketItem.OperationType = "INSERT";
                    newQueryBucketItem.DestinationTable = "EntityTypes";
                    newQueryBucketItem.QueryLog = JsonConvert.SerializeObject(_mapper.Map<ModeratorEntityTypeDto>(returnedObject), serializerSettings);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                    return returnedObject.TypeId;
                }
                catch (Exception ex)
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task DeleteEntityTypeAsync(uint typeId)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    newQueryBucketItem.OperationType = "DELETE";
                    newQueryBucketItem.DestinationTable = "EntityTypes";
                    newQueryBucketItem.QueryLog = typeId.ToString();
                    await _entityTypeRepository.DeleteEntityTypeAsync(typeId);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                }
                catch (Exception ex) 
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<List<EntityLanguageGetDto>> GetLanguagesAsync()
        {
            var entityType = await _entityLanguageRepository.GetLanguagesAsync();
            return _mapper.Map<List<EntityLanguageGetDto>>(entityType);
        }
        public async Task<uint> AddLanguageAsync(string langCode)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    EntityLanguage entityLanguageToAdd = new EntityLanguage();
                    entityLanguageToAdd.LangValue = langCode;
                    entityLanguageToAdd.LangId = await _entityLanguageRepository.AddLanguageAsync(entityLanguageToAdd);
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    newQueryBucketItem.OperationType = "INSERT";
                    newQueryBucketItem.DestinationTable = "EntityLanguages";
                    newQueryBucketItem.QueryLog = JsonConvert.SerializeObject(_mapper.Map<ModeratorEntityLanguageDto>(entityLanguageToAdd), serializerSettings); ;
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                    return entityLanguageToAdd.LangId;
                }
                catch (Exception ex) 
                { 
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task DeleteLanguageAsync(uint langId)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    newQueryBucketItem.OperationType = "DELETE";
                    newQueryBucketItem.DestinationTable = "EntityLanguages";
                    newQueryBucketItem.QueryLog = langId.ToString();
                    await _entityLanguageRepository.DeleteLanguageAsync(langId);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                }
                catch(Exception ex) 
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task UpdateLanguageAsync(EntityLanguageUpdateDto entityLanguageUpdate)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    var mappedObject = _mapper.Map<EntityLanguage>(entityLanguageUpdate);
                    newQueryBucketItem.OperationType = "UPDATE";
                    newQueryBucketItem.DestinationTable = "EntityLanguages";
                    newQueryBucketItem.QueryLog = JsonConvert.SerializeObject(_mapper.Map<ModeratorEntityLanguageDto>(mappedObject), serializerSettings);
                    await _entityLanguageRepository.UpdateLanguageAsync(mappedObject);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                }
                catch(Exception ex) 
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<uint> AddEntityAsync(uint? parentId, EntityPostDto newEntity)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {

                    var returnedObject = await _entityRepository.AddEntityAsync(_mapper.Map<Entity>(newEntity));
                    uint newEntityId = returnedObject.EntityId;
                    EntitiesGraphDatum newEntityRelation = new EntitiesGraphDatum();
                    newEntityRelation.ParentId = parentId;
                    newEntityRelation.ChildId = newEntityId;
                    var returnedRelation = await _entityGraphDataRepository.AttachEntityToParentAsync(newEntityRelation);

                    ModeratorQueryBucket newQueryBucketItemChild = new ModeratorQueryBucket();
                    newQueryBucketItemChild.OperationType = "INSERT";
                    newQueryBucketItemChild.DestinationTable = "Entities";
                    newQueryBucketItemChild.QueryLog = JsonConvert.SerializeObject(_mapper.Map<ModeratorEntityDto>(returnedObject), serializerSettings);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItemChild);

                    ModeratorQueryBucket newQueryBucketItemRelation = new ModeratorQueryBucket();
                    newQueryBucketItemRelation.OperationType = "INSERT";
                    newQueryBucketItemRelation.DestinationTable = "EntitiesGraphData";
                    newQueryBucketItemRelation.QueryLog = JsonConvert.SerializeObject(_mapper.Map<ModeratorEntityGraphDataDto>(returnedRelation), serializerSettings);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItemRelation);
                    List<Entity> entityWrapper = new List<Entity>();
                    var returnedParentEntity = await _entityRepository.MarkEntityHasChilds(parentId);
                    entityWrapper.Add(returnedParentEntity);
                    ModeratorQueryBucket newQueryBucketItemParent = new ModeratorQueryBucket();
                    newQueryBucketItemParent.OperationType = "UPDATE";
                    newQueryBucketItemParent.DestinationTable = "Entities";
                    newQueryBucketItemParent.QueryLog = JsonConvert.SerializeObject(_mapper.Map<List<ModeratorEntityDto>>(entityWrapper), serializerSettings);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItemParent);

                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                    return newEntityId;
                }
                catch(Exception ex)
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task DeleteEntityAsync(uint entityId)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    newQueryBucketItem.OperationType = "DELETE";
                    newQueryBucketItem.DestinationTable = "Entities";
                    newQueryBucketItem.QueryLog = entityId.ToString();
                    await _entityRepository.DeleteEntityAsync(entityId);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                }
                catch(Exception ex) 
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task UpdateEntityOrderAsync(List<EntityUpdateDto> entity)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    var mappedObject = _mapper.Map<List<Entity>>(entity);
                    newQueryBucketItem.OperationType = "UPDATE";
                    newQueryBucketItem.DestinationTable = "Entities";
                    newQueryBucketItem.QueryLog = JsonConvert.SerializeObject(_mapper.Map<List<ModeratorEntityDto>>(mappedObject), serializerSettings);
                    await _entityRepository.UpdateEntityOrderAsync(mappedObject);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);

                }
                catch (Exception ex)
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
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
            if (!entityList.Any()) throw new NotFoundException($"Specified entity has no childs or not exists: {entityId}");
            return _mapper.Map<List<EntityWithTranslationDto>>(entityList);
        }

        public async Task<uint> AddEntityTranslationAsync(EntityTranslationPostDto newEntityTranslation)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    var returnedObject = await _entityTranslationsRepository.AddEntityTranslationAsync(_mapper.Map<EntityTranslation>(newEntityTranslation));
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    newQueryBucketItem.OperationType = "INSERT";
                    newQueryBucketItem.DestinationTable = "EntityTranslations";
                    newQueryBucketItem.QueryLog = JsonConvert.SerializeObject(_mapper.Map<ModeratorEntityTranslationDto>(returnedObject), serializerSettings);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                    return returnedObject.TranslationId;
                }
                catch (Exception ex)
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task DeleteEntityTranslationAsync(uint entityTranslationId)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    newQueryBucketItem.OperationType = "DELETE";
                    newQueryBucketItem.DestinationTable = "EntityTranslations";
                    newQueryBucketItem.QueryLog = entityTranslationId.ToString();
                    await _entityTranslationsRepository.DeleteEntityTranslationAsync(entityTranslationId);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                }
                catch (Exception ex) 
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task UpdateEntityTranslationAsync(EntityTranslationUpdateDto entityTranslation)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    var mappedObject = _mapper.Map<EntityTranslation>(entityTranslation);
                    newQueryBucketItem.OperationType = "UPDATE";
                    newQueryBucketItem.DestinationTable = "EntityTranslations";
                    newQueryBucketItem.QueryLog = JsonConvert.SerializeObject(_mapper.Map<ModeratorEntityTranslationDto>(mappedObject), serializerSettings);
                    await _entityTranslationsRepository.UpdateEntityTranslationAsync(mappedObject);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                }
                catch(Exception ex) 
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<List<EntityTranslationDto>> GetEntityTranslationsAsync()
        {
            return _mapper.Map<List<EntityTranslationDto>>(await _entityTranslationsRepository.GetEntityTranslationsAsync());
        }


        public async Task<uint> AddEntityTranslationFileAsync(EntityTranslationFilePostDto newEntityTranslationFile)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    EntityTranslationFile entityTranslationFile = new EntityTranslationFile();
                    var saveResult = await _fileService.SaveFileAsync(newEntityTranslationFile.File);
                    entityTranslationFile.FilePath = saveResult.Item1;
                    entityTranslationFile.TranslationId = newEntityTranslationFile.TranslationId;
                    entityTranslationFile.FilePathHashed = saveResult.Item3;
                    entityTranslationFile.FilePriority = newEntityTranslationFile.FilePriority;
                    entityTranslationFile.FileType = saveResult.Item2;
                    entityTranslationFile.ReferredField = newEntityTranslationFile.ReferredField;
                    var returnedObject = await _entityTranslationFilesRepository.AddEntityTranslationFileAsync(entityTranslationFile);
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    newQueryBucketItem.OperationType = "INSERT";
                    newQueryBucketItem.DestinationTable = "EntityTranslationFiles";
                    newQueryBucketItem.QueryLog = JsonConvert.SerializeObject(_mapper.Map<ModeratorEntityTranslationFileDto>(returnedObject), serializerSettings);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                    return returnedObject.FileId;
                }
                catch (Exception ex) 
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task DeleteEntityTranslationFileAsync(uint entityTranslationFileId)
        {
            using (var transaction = await _moderatorTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    ModeratorQueryBucket newQueryBucketItem = new ModeratorQueryBucket();
                    newQueryBucketItem.OperationType = "DELETE";
                    newQueryBucketItem.DestinationTable = "EntityTranslationFiles";
                    newQueryBucketItem.QueryLog = entityTranslationFileId.ToString();
                    string actualFilePath = await _entityTranslationFilesRepository.GetActualFilePathAsync(entityTranslationFileId);
                    if (actualFilePath == null) throw new ArgumentException();
                    await _fileService.DeleteFileAsync(actualFilePath);
                    await _entityTranslationFilesRepository.DeleteEntityTranslationFileAsync(entityTranslationFileId);
                    await _moderatorQueryBuckerRepository.AddBucketItemAsync(newQueryBucketItem);
                    await _moderatorTransactionRepository.CommitTransactionAsync(transaction);
                }
                catch (Exception ex) 
                {
                    await _moderatorTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }

        }

        public async Task<Stream> GetEntityTranslationFileAsync(string filePath)
        {
            string actualFileName = await _entityTranslationFilesRepository.GetActualFilePathAsync(filePath);
            return await _fileService.GetFileStreamAsync(actualFileName);
        }

        public async Task<bool> CheckIfWeHaveChangesAsync()
        {
            return await _moderatorQueryBuckerRepository.CheckIfWeHaveChangesAsync();
        }

        public async Task RefreshEntityLockAsync(uint entityId, uint newTime)
        {
            if(newTime < 0 || newTime > 600) throw new ArgumentOutOfRangeException();
            await _entityRepository.RefreshEntityLockAsync(entityId, newTime);
        }

        public async Task<bool> IsEntityLockedAsync(uint entityId)
        {
            return await _entityRepository.IsEntityLockedAsync(entityId);
        }
    }
}
