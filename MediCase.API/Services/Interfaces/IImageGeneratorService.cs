namespace MediCase.API.Services.Interfaces
{
    public interface IImageGeneratorService
    {
        Task GenerateTranslationFileAsync(uint nodeId, string? customPrompt);
    }
}
