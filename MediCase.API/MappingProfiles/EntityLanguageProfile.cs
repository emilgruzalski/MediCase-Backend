using AutoMapper;
using MediCase.API.Entities.Content;
using MediCase.API.Entities.Moderator;
using MediCase.API.Models.Entity;

namespace MediCase.API.MappingProfiles
{
    public class EntityLanguageProfile : Profile
    {
        public EntityLanguageProfile()
        {
            CreateMap<EntityLanguageGetDto, EntityLanguage>();
            CreateMap<EntityLanguageGetDto, Entities.Content.EntityLanguage>();

            CreateMap<EntityLanguage, EntityLanguageGetDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate());

            CreateMap<Entities.Content.EntityLanguage, EntityLanguageGetDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate());

            CreateMap<EntityLanguageUpdateDto, EntityLanguage>();
            CreateMap<EntityLanguageUpdateDto, Entities.Content.EntityLanguage>();
        }
    }
}
