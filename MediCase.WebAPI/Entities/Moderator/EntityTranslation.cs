using System;
using System.Collections.Generic;

namespace MediCase.WebAPI.Entities.Moderator;

public partial class EntityTranslation
{
    public uint TranslationId { get; set; }

    public uint EntityId { get; set; }

    public string? MainTitle { get; set; }

    public string? SubTitle { get; set; }

    public string? Paragrahps { get; set; }

    public uint LangId { get; set; }

    public virtual Entity Entity { get; set; } = null!;

    public virtual ICollection<EntityTranslationFile> EntityTranslationFiles { get; set; } = new List<EntityTranslationFile>();

    public virtual EntityLanguage Lang { get; set; } = null!;
}
