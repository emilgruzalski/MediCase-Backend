using Newtonsoft.Json;

namespace MediCase.API.Models.Entity
{
    public class EntityWithTranslationDto
    {
        public uint EntityId { get; set; }

        public uint TypeId { get; set; }

        public ulong EntityOrder { get; set; }

        public bool HasChilds { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsLocked { get; set; }
        public List<EntityTranslationDto>? EntityTranslations { get; set; }
    }
}
