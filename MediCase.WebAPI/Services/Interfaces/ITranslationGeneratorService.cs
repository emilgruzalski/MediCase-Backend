using MediCase.WebAPI.Models.Entity;

namespace MediCase.WebAPI.Services.Interfaces
{
    public interface ITranslationGeneratorService
    {
        Task<EntityTranslationDto> GenerateTranslationAsync(uint translationId, uint desiredLanguageId);
    }
}
