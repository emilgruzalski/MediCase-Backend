using System;
using System.Collections.Generic;

namespace MediCase.WebAPI.Entities.Content;

public partial class EntityType
{
    public uint TypeId { get; set; }

    public string TypeValue { get; set; } = null!;

    public virtual ICollection<Entity> Entities { get; set; } = new List<Entity>();
}
