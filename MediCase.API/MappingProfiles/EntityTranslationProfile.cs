using AutoMapper;
using MediCase.API.Entities.Content;
using MediCase.API.Models.Entity;

namespace MediCase.API.MappingProfiles
{
    public class EntityTranslationProfile : Profile
    {
        public EntityTranslationProfile()
        {
            CreateMap<EntityTranslationDto, Entities.Moderator.EntityTranslation>();
            CreateMap<EntityTranslationDto, EntityTranslation>();

            CreateMap<Entities.Moderator.EntityTranslation, EntityTranslationDto>()
                .ForSourceMember(src => src.Entity, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Lang, opt => opt.DoNotValidate())
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.EntityTranslationFiles));
            CreateMap<EntityTranslation, EntityTranslationDto>()
                .ForSourceMember(src => src.Entity, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Lang, opt => opt.DoNotValidate())
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.EntityTranslationFiles));

            CreateMap<EntityTranslationPostDto, Entities.Moderator.EntityTranslation>();
            CreateMap<EntityTranslationPostDto, EntityTranslation>();

            CreateMap<Entities.Moderator.EntityTranslation, EntityTranslationPostDto>()
                .ForSourceMember(src => src.Entity, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Lang, opt => opt.DoNotValidate());
            CreateMap<EntityTranslation, EntityTranslationPostDto>()
                .ForSourceMember(src => src.Entity, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Lang, opt => opt.DoNotValidate());

            CreateMap<EntityTranslationUpdateDto, Entities.Moderator.EntityTranslation>();
            CreateMap<EntityTranslationUpdateDto, EntityTranslation>();
        }
    }
}
