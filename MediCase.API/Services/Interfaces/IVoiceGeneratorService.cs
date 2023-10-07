namespace MediCase.API.Services.Interfaces
{
    public interface IVoiceGeneratorService
    {
        Task GenerateVoiceAsync(uint translationId, string? refferedField);
        Task<List<string>> GetSupportedLanguagesAsync();
    }
}
