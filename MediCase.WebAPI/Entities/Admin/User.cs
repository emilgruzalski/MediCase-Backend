using System;
using System.Collections.Generic;

namespace MediCase.WebAPI.Entities.Admin;

public partial class User
{
    public uint Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
