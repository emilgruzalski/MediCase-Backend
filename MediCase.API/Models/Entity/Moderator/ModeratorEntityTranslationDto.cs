namespace MediCase.API.Models.Entity.Moderator
{
    public class ModeratorEntityTranslationDto
    {
        public uint TranslationId { get; set; }

        public uint EntityId { get; set; }

        public string? MainTitle { get; set; }

        public string? SubTitle { get; set; }

        public string? Paragrahps { get; set; }

        public uint LangId { get; set; }
    }
}
