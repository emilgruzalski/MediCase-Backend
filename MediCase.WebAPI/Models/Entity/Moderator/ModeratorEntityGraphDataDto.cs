namespace MediCase.WebAPI.Models.Entity.Moderator
{
    public class ModeratorEntityGraphDataDto
    {
        public uint EdgeId { get; set; }

        public uint? ParentId { get; set; }

        public uint ChildId { get; set; }
    }
}
