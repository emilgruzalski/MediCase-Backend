using AutoMapper;
using MediCase.WebAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace MediCase.WebAPI.MapperProfiles
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<EntityDto, Entities.Moderator.Entity>();
            CreateMap<EntityDto, Entities.Content.Entity>();

            CreateMap<Entities.Moderator.Entity, EntityDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate())
                .ForMember(dest => dest.HasChilds, opt => opt.MapFrom(src => src.HasChilds))
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => (src.LockExpirationDate <= DateTime.UtcNow.AddSeconds(5)) ? false : true)); ;
            CreateMap<Entities.Content.Entity, EntityDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate())
                .ForMember(dest => dest.HasChilds, opt => opt.MapFrom(src => src.HasChilds));

            CreateMap<EntityWithTranslationDto, Entities.Moderator.Entity>();
            CreateMap<EntityWithTranslationDto, Entities.Content.Entity>();

            CreateMap<Entities.Moderator.Entity, EntityWithTranslationDto>()
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate())
                .ForMember(dest => dest.EntityOrder, opt => opt.MapFrom(src => src.EntityOrder))
                .ForMember(dest => dest.HasChilds, opt => opt.MapFrom(src => src.HasChilds))
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => (src.LockExpirationDate <= DateTime.UtcNow.AddSeconds(5)) ? false : true));

            CreateMap<Entities.Content.Entity, EntityWithTranslationDto>()
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate())
                .ForMember(dest => dest.EntityOrder, opt => opt.MapFrom(src => src.EntityOrder))
                .ForMember(dest => dest.HasChilds, opt => opt.MapFrom(src => src.HasChilds));

            CreateMap<EntityPostDto, Entities.Moderator.Entity>();
            CreateMap<EntityPostDto, Entities.Content.Entity>();

            CreateMap<Entities.Moderator.Entity, EntityPostDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate());
            CreateMap<Entities.Content.Entity, EntityPostDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate());

            CreateMap<EntityUpdateDto, Entities.Moderator.Entity>();
            CreateMap<EntityUpdateDto, Entities.Content.Entity>();

        }
    }
}
