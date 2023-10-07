using AutoMapper;
using MediCase.API.Entities.Content;
using MediCase.API.Entities.Moderator;
using MediCase.API.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace MediCase.API.MappingProfiles
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<EntityDto, Entity>();
            CreateMap<EntityDto, Entities.Content.Entity>();

            CreateMap<Entity, EntityDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate())
                .ForMember(dest => dest.HasChilds, opt => opt.MapFrom(src => src.HasChilds))
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => src.LockExpirationDate <= DateTime.UtcNow.AddSeconds(5) ? false : true)); ;
            CreateMap<Entities.Content.Entity, EntityDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate())
                .ForMember(dest => dest.HasChilds, opt => opt.MapFrom(src => src.HasChilds));

            CreateMap<EntityWithTranslationDto, Entity>();
            CreateMap<EntityWithTranslationDto, Entities.Content.Entity>();

            CreateMap<Entity, EntityWithTranslationDto>()
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate())
                .ForMember(dest => dest.EntityOrder, opt => opt.MapFrom(src => src.EntityOrder))
                .ForMember(dest => dest.HasChilds, opt => opt.MapFrom(src => src.HasChilds))
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => src.LockExpirationDate <= DateTime.UtcNow.AddSeconds(5) ? false : true));

            CreateMap<Entities.Content.Entity, EntityWithTranslationDto>()
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate())
                .ForMember(dest => dest.EntityOrder, opt => opt.MapFrom(src => src.EntityOrder))
                .ForMember(dest => dest.HasChilds, opt => opt.MapFrom(src => src.HasChilds));

            CreateMap<EntityPostDto, Entity>();
            CreateMap<EntityPostDto, Entities.Content.Entity>();

            CreateMap<Entity, EntityPostDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate());
            CreateMap<Entities.Content.Entity, EntityPostDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate());

            CreateMap<EntityUpdateDto, Entity>();
            CreateMap<EntityUpdateDto, Entities.Content.Entity>();

        }
    }
}
