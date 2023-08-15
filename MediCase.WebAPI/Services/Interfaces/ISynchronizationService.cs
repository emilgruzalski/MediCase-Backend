namespace MediCase.WebAPI.Services.Interfaces
{
    public interface ISynchronizationService
    {
        Task SynchronizeDatabasesAsync();
    }
}