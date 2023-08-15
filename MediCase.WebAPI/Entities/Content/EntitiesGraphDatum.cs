using System;
using System.Collections.Generic;

namespace MediCase.WebAPI.Entities.Content;

public partial class EntitiesGraphDatum
{
    public uint EdgeId { get; set; }

    public uint? ParentId { get; set; }

    public uint ChildId { get; set; }

    public virtual Entity Child { get; set; } = null!;

    public virtual Entity? Parent { get; set; }
}
