namespace MediCase.WebAPI.Services.Interfaces
{
    public interface IFileService
    {
        Task<Tuple<string, string, string>> SaveFileAsync(IFormFile newFile);

        Task<FileStream> GetFileStreamAsync(string fileName);
        Task DeleteFileAsync(string fileName);

    }
}