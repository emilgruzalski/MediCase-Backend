using AutoMapper;
using MediCase.WebAPI.Models.Entity;

namespace MediCase.WebAPI.MapperProfiles
{
    public class EntityLanguageProfile : Profile
    {
        public EntityLanguageProfile() 
        {
            CreateMap<EntityLanguageGetDto, Entities.Moderator.EntityLanguage>();
            CreateMap<EntityLanguageGetDto, Entities.Content.EntityLanguage>();

            CreateMap<Entities.Moderator.EntityLanguage, EntityLanguageGetDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate());

            CreateMap<Entities.Content.EntityLanguage, EntityLanguageGetDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate());

            CreateMap<EntityLanguageUpdateDto, Entities.Moderator.EntityLanguage>();
            CreateMap<EntityLanguageUpdateDto, Entities.Content.EntityLanguage>();
        }
    }
}
