namespace MediCase.WebAPI.Models.Group
{
    public class GroupDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateOnly ExpirationDate { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsModerator { get; set; }
        public bool IsUser { get; set; }
    }
}
