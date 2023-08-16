using MediCase.WebAPI.Entities.Admin;

namespace MediCase.WebAPI.Models.Group
{
    public class GetGroupDto
    {
        public uint Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsModerator { get; set; }
        public bool IsUser { get; set; }
        public int UsersCount { get; set; }
    }
}
