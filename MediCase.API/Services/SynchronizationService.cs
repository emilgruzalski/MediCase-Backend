using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using AutoMapper;
using MediCase.API.Models.Entity.Moderator;
using MediCase.API.Repositories.Moderator.Interfaces;
using MediCase.API.Entities.Content;
using MediCase.API.Repositories.Content.Interfaces;
using MediCase.API.Services.Interfaces;

namespace MediCase.API.Services
{
    public class SynchronizationService : ISynchronizationService
    {
        private readonly ISynchronizationRepository _synchronizationRepository;
        private readonly IModeratorQueryBucketRepository _moderatorQueryBucketRepository;
        private readonly IMediCaseTransactionRepository _medicaseEntitiesTransactionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public SynchronizationService(
            ISynchronizationRepository synchronizationRepository,
            IMediCaseTransactionRepository transactionRepository,
            IModeratorQueryBucketRepository moderatorQueryBucketRepository,
            ILogger<SynchronizationService> logger,
            IMapper mapper)
        {
            _synchronizationRepository = synchronizationRepository;
            _moderatorQueryBucketRepository = moderatorQueryBucketRepository;
            _logger = logger;
            _mapper = mapper;
            _medicaseEntitiesTransactionRepository = transactionRepository;
        }

        public async Task SynchronizeDatabasesAsync()
        {
            var queriesQueue = await _moderatorQueryBucketRepository.GetPendingChangesAsync();
            using (var transaction = await _medicaseEntitiesTransactionRepository.BeginTransactionAsync())
            {
                try
                {
                    foreach (var query in queriesQueue)
                    {
                        switch (query.DestinationTable)
                        {
                            case "Entities":
                                switch (query.OperationType)
                                {
                                    case "INSERT":
                                        var newEntity = JsonSerializer.Deserialize<ModeratorEntityDto?>(query.QueryLog);
                                        if (newEntity != null)
                                        {
                                            await _synchronizationRepository.ImportEntityAsync(_mapper.Map<Entity>(newEntity));
                                        }
                                        break;
                                    case "DELETE":
                                        uint entityId;
                                        if (uint.TryParse(query.QueryLog, out entityId))
                                        {
                                            await _synchronizationRepository.DeleteImportedEntityAsync(entityId);
                                        }
                                        break;
                                    case "UPDATE":
                                        var entities = JsonSerializer.Deserialize<List<ModeratorEntityDto>?>(query.QueryLog);
                                        if (entities != null)
                                        {
                                            await _synchronizationRepository.UpdateImportedEntitiesAsync(_mapper.Map<List<Entity>>(entities));
                                        }
                                        break;
                                    default:
                                        break;

                                }
                                break;
                            case "EntitiesGraphData":
                                switch (query.OperationType)
                                {
                                    case "INSERT":
                                        var entityRelation = JsonSerializer.Deserialize<ModeratorEntityGraphDataDto>(query.QueryLog);
                                        if (entityRelation != null)
                                        {
                                            await _synchronizationRepository.ImportEntityGraphRelationAsync(_mapper.Map<EntitiesGraphDatum>(entityRelation));
                                        }
                                        break;
                                    default:
                                        break;

                                }
                                break;
                            case "EntityLanguages":
                                switch (query.OperationType)
                                {
                                    case "INSERT":
                                        var entityLanguage = JsonSerializer.Deserialize<ModeratorEntityLanguageDto>(query.QueryLog);
                                        if (entityLanguage != null)
                                        {
                                            await _synchronizationRepository.ImportEntityLanguageAsync(_mapper.Map<EntityLanguage>(entityLanguage));
                                        }
                                        break;
                                    case "DELETE":
                                        uint langId;
                                        if (uint.TryParse(query.QueryLog, out langId))
                                        {
                                            await _synchronizationRepository.DeleteImportedEntityLanguageAsync(langId);
                                        }
                                        break;
                                    case "UPDATE":
                                        var entityLanguageToUpdate = JsonSerializer.Deserialize<ModeratorEntityLanguageDto>(query.QueryLog);
                                        if (entityLanguageToUpdate != null)
                                        {
                                            await _synchronizationRepository.UpdateImportedEntityLanguageAsync(_mapper.Map<EntityLanguage>(entityLanguageToUpdate));
                                        }
                                        break;
                                    default:
                                        break;

                                }
                                break;
                            case "EntityTranslations":
                                switch (query.OperationType)
                                {
                                    case "INSERT":
                                        var newEntityTranslation = JsonSerializer.Deserialize<ModeratorEntityTranslationDto>(query.QueryLog);
                                        if (newEntityTranslation != null)
                                        {
                                            await _synchronizationRepository.ImportEntityTranslationAsync(_mapper.Map<EntityTranslation>(newEntityTranslation));
                                        }
                                        break;
                                    case "DELETE":
                                        uint entityTranslationId;
                                        if (uint.TryParse(query.QueryLog, out entityTranslationId))
                                        {
                                            await _synchronizationRepository.DeleteImportedEntityTranslationAsync(entityTranslationId);
                                        }
                                        break;
                                    case "UPDATE":
                                        var entityTranslationToUpdate = JsonSerializer.Deserialize<ModeratorEntityTranslationDto>(query.QueryLog);
                                        if (entityTranslationToUpdate != null)
                                        {
                                            await _synchronizationRepository.UpdateImportedEntityTranslationAsync(_mapper.Map<EntityTranslation>(entityTranslationToUpdate));
                                        }
                                        break;
                                    default:
                                        break;

                                }
                                break;
                            case "EntityTypes":
                                switch (query.OperationType)
                                {
                                    case "INSERT":
                                        var newEntityType = JsonSerializer.Deserialize<ModeratorEntityTypeDto>(query.QueryLog);
                                        if (newEntityType != null)
                                        {
                                            await _synchronizationRepository.ImportEntityTypeAsync(_mapper.Map<EntityType>(newEntityType));
                                        }
                                        break;
                                    case "DELETE":
                                        uint entityTypeId;
                                        if (uint.TryParse(query.QueryLog, out entityTypeId))
                                        {
                                            await _synchronizationRepository.DeleteImportedEntityTranslationAsync(entityTypeId);
                                        }
                                        break;
                                    case "UPDATE":
                                        var entityTypeToUpdate = JsonSerializer.Deserialize<ModeratorEntityTypeDto>(query.QueryLog);
                                        if (entityTypeToUpdate != null)
                                        {
                                            await _synchronizationRepository.ImportEntityTypeAsync(_mapper.Map<EntityType>(entityTypeToUpdate));
                                        }
                                        break;
                                    default:
                                        break;

                                }
                                break;
                            case "EntityTranslationFiles":
                                switch (query.OperationType)
                                {
                                    case "INSERT":
                                        var newEntityTranslationFile = JsonSerializer.Deserialize<ModeratorEntityTranslationFileDto>(query.QueryLog);
                                        if (newEntityTranslationFile != null)
                                        {
                                            await _synchronizationRepository.ImportEntityTranslationFileAsync(_mapper.Map<EntityTranslationFile>(newEntityTranslationFile));
                                        }
                                        break;
                                    case "DELETE":
                                        uint fileId;
                                        if (uint.TryParse(query.QueryLog, out fileId))
                                        {
                                            await _synchronizationRepository.DeleteImportedEntityTranslationFileAsync(fileId);
                                        }
                                        break;
                                    case "UPDATE":
                                        var entityTranslationFileToUpdate = JsonSerializer.Deserialize<ModeratorEntityTranslationFileDto>(query.QueryLog);
                                        if (entityTranslationFileToUpdate != null)
                                        {
                                            await _synchronizationRepository.UpdateImportedEntityTranslationFileAsync(_mapper.Map<EntityTranslationFile>(entityTranslationFileToUpdate));
                                        }
                                        break;
                                    default:
                                        break;

                                }
                                break;
                            default:
                                break;
                        }
                    }
                    await _moderatorQueryBucketRepository.TrucateTableAsync();
                    await _medicaseEntitiesTransactionRepository.CommitTransactionAsync(transaction);
                }
                catch (Exception ex)
                {
                    await _medicaseEntitiesTransactionRepository.RollbackTransactionAsync(transaction);
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
