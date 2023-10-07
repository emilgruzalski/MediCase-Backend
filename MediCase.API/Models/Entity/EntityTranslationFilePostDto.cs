namespace MediCase.API.Models.Entity
{
    public class EntityTranslationFilePostDto
    {
        public uint TranslationId { get; set; }

        public IFormFile File { get; set; } = null!;

        public string ReferredField { get; set; } = null!;

        public uint FilePriority { get; set; }
    }
}
