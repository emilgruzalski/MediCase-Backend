using AutoMapper;
using MediCase.WebAPI.Models.Entity.Moderator;

namespace MediCase.WebAPI.MappingProfiles
{
    public class ModeratorEntitiesMappingProfile : Profile
    {
        public ModeratorEntitiesMappingProfile() 
        {
            CreateMap<Entities.Moderator.EntityTranslation, ModeratorEntityTranslationDto>()
                .ForSourceMember(src => src.EntityTranslationFiles, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Lang, opt => opt.DoNotValidate());
            CreateMap<ModeratorEntityTranslationDto, Entities.Content.EntityTranslation>();


            CreateMap<Entities.Moderator.Entity, ModeratorEntityDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate());
            CreateMap<ModeratorEntityDto, Entities.Content.Entity>();

            CreateMap<Entities.Moderator.EntityLanguage, ModeratorEntityLanguageDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate());
            CreateMap<ModeratorEntityLanguageDto, Entities.Content.EntityLanguage>();

            CreateMap<Entities.Moderator.EntityType, ModeratorEntityTypeDto>()
                .ForSourceMember(src => src.Entities, opt => opt.DoNotValidate());
            CreateMap<ModeratorEntityTypeDto, Entities.Content.EntityType>();

            CreateMap<Entities.Moderator.EntityTranslationFile, ModeratorEntityTranslationFileDto>()
                .ForSourceMember(src => src.Translation, opt => opt.DoNotValidate());
            CreateMap<ModeratorEntityTranslationFileDto, Entities.Content.EntityTranslationFile>();

            CreateMap<Entities.Moderator.EntitiesGraphDatum, ModeratorEntityGraphDataDto>()
                .ForSourceMember(src => src.Child, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Parent, opt => opt.DoNotValidate());
            CreateMap<ModeratorEntityGraphDataDto, Entities.Content.EntitiesGraphDatum>();
        }
    }
}
