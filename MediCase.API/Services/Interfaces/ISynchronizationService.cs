namespace MediCase.API.Services.Interfaces
{
    public interface ISynchronizationService
    {
        Task SynchronizeDatabasesAsync();
    }
}