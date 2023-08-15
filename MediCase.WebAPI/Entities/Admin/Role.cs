using System;
using System.Collections.Generic;

namespace MediCase.WebAPI.Entities.Admin;

public partial class Role
{
    public uint Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
