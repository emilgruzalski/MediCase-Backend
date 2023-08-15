using MediCase.WebAPI.Entities.Moderator;

namespace MediCase.WebAPI.Repositories.Moderator.Interfaces
{
    public interface IEntityTranslationFilesRepository
    {
        Task<EntityTranslationFile> AddEntityTranslationFileAsync(EntityTranslationFile newEntityTranslationFile);
        Task DeleteEntityTranslationFileAsync(uint entityTranslationFile);

        Task UpdateEntityTranslationFileAsync(EntityTranslationFile entityTranslationFile);
        Task<uint> GetNextFileIdAsync();

        Task<string> GetActualFilePathAsync(string hashedFileName);

        Task<string> GetActualFilePathAsync(uint fileId);

    }
}
