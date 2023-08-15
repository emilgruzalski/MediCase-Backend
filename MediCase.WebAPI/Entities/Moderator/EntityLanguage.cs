using System;
using System.Collections.Generic;

namespace MediCase.WebAPI.Entities.Moderator;

public partial class EntityLanguage
{
    public uint LangId { get; set; }

    public string LangValue { get; set; } = null!;

    public virtual ICollection<EntityTranslation> EntityTranslations { get; set; } = new List<EntityTranslation>();
}
