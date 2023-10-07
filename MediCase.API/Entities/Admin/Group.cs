using System;
using System.Collections.Generic;

namespace MediCase.API.Entities.Admin;

public partial class Group
{
    public uint Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsModerator { get; set; }

    public bool IsUser { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
