using System;
using System.Collections.Generic;

namespace MediCase.API.Entities.Content;

public partial class Entity
{
    public uint EntityId { get; set; }

    public uint TypeId { get; set; }

    public ulong EntityOrder { get; set; }

    public bool HasChilds { get; set; }

    public DateTime LockExpirationDate { get; set; }

    public virtual ICollection<EntitiesGraphDatum> EntitiesGraphDatumChildren { get; set; } = new List<EntitiesGraphDatum>();

    public virtual ICollection<EntitiesGraphDatum> EntitiesGraphDatumParents { get; set; } = new List<EntitiesGraphDatum>();

    public virtual ICollection<EntityTranslation> EntityTranslations { get; set; } = new List<EntityTranslation>();

    public virtual EntityType Type { get; set; } = null!;
}
