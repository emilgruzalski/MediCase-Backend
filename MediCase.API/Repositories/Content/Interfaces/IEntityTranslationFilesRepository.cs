namespace MediCase.API.Repositories.Content.Interfaces
{
    public interface IEntityTranslationFilesRepository
    {
        Task<string> GetActualFilePathAsync(string hashedFileName);
        Task<string> GetActualFilePathAsync(uint fileId);
    }
}
