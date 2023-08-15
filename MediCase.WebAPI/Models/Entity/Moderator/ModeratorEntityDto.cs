namespace MediCase.WebAPI.Models.Entity.Moderator
{
    public class ModeratorEntityDto
    {
        public uint EntityId { get; set; }

        public uint TypeId { get; set; }

        public ulong EntityOrder { get; set; }

        public bool HasChilds { get; set; }

        public DateTime LockExpirationDate { get; set; }
    }
}
