namespace MediCase.WebAPI.Models.Entity.Moderator
{
    public class ModeratorEntityTranslationFileDto
    {
        public uint FileId { get; set; }

        public uint TranslationId { get; set; }

        public string FileType { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        public string FilePathHashed { get; set; } = null!;

        public string ReferredField { get; set; } = null!;

        public uint FilePriority { get; set; }
    }
}
