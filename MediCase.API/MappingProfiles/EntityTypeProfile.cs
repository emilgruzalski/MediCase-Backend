using AutoMapper;
using MediCase.API.Models.Entity;

namespace MediCase.API.MappingProfiles
{
    public class EntityTypeProfile : Profile
    {
        public EntityTypeProfile()
        {
            CreateMap<EntityTypeDto, Entities.Moderator.EntityType>();
            CreateMap<EntityTypeDto, Entities.Content.EntityType>();

            CreateMap<Entities.Moderator.EntityType, EntityTypeDto>()
                .ForSourceMember(src => src.Entities, opt => opt.DoNotValidate());
            CreateMap<Entities.Content.EntityType, EntityTypeDto>()
                .ForSourceMember(src => src.Entities, opt => opt.DoNotValidate());
        }
    }
}
