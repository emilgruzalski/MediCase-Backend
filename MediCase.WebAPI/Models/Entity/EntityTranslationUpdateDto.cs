namespace MediCase.WebAPI.Models.Entity
{
    public class EntityTranslationUpdateDto
    {
        public uint TranslationId { get; set; }
 
        public string? MainTitle { get; set; }

        public string? SubTitle { get; set; }

        public string? Paragrahps { get; set; }

        public uint LangId { get; set; }
    }
}
