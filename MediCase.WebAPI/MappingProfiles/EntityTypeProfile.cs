using AutoMapper;
using MediCase.WebAPI.Models.Entity;

namespace MediCase.WebAPI.MapperProfiles
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
