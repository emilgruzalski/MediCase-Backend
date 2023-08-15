using AutoMapper;
using MediCase.WebAPI.Models.Entity;

namespace MediCase.WebAPI.MapperProfiles
{
    public class EntityTranslationProfile : Profile
    {
        public EntityTranslationProfile()
        {
            CreateMap<EntityTranslationDto, Entities.Moderator.EntityTranslation>();
            CreateMap<EntityTranslationDto, Entities.Content.EntityTranslation>();

            CreateMap<Entities.Moderator.EntityTranslation, EntityTranslationDto>()
                .ForSourceMember(src => src.Entity, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Lang, opt => opt.DoNotValidate())
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.EntityTranslationFiles));
            CreateMap<Entities.Content.EntityTranslation, EntityTranslationDto>()
                .ForSourceMember(src => src.Entity, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Lang, opt => opt.DoNotValidate())
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.EntityTranslationFiles));

            CreateMap<EntityTranslationPostDto, Entities.Moderator.EntityTranslation>();
            CreateMap<EntityTranslationPostDto, Entities.Content.EntityTranslation>();

            CreateMap<Entities.Moderator.EntityTranslation, EntityTranslationPostDto>()
                .ForSourceMember(src => src.Entity, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Lang, opt => opt.DoNotValidate());
            CreateMap<Entities.Content.EntityTranslation, EntityTranslationPostDto>()
                .ForSourceMember(src => src.Entity, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Lang, opt => opt.DoNotValidate());

            CreateMap<EntityTranslationUpdateDto, Entities.Moderator.EntityTranslation>();
            CreateMap<EntityTranslationUpdateDto, Entities.Content.EntityTranslation>();
        }
    }
}
