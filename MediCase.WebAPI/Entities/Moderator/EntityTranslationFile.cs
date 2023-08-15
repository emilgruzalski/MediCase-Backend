using System;
using System.Collections.Generic;

namespace MediCase.WebAPI.Entities.Moderator;

public partial class EntityTranslationFile
{
    public uint FileId { get; set; }

    public uint TranslationId { get; set; }

    public string FileType { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string FilePathHashed { get; set; } = null!;

    public string ReferredField { get; set; } = null!;

    public uint FilePriority { get; set; }

    public virtual EntityTranslation Translation { get; set; } = null!;
}
