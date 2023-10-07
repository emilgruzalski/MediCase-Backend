using MediCase.API.Models.Entity;

namespace MediCase.API.Services.Interfaces
{
    public interface ITranslationGeneratorService
    {
        Task<EntityTranslationDto> GenerateTranslationAsync(uint translationId, uint desiredLanguageId);
    }
}
