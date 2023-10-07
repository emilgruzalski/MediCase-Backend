using AutoMapper;
using MediCase.API.Entities.Content;
using MediCase.API.Entities.Moderator;
using MediCase.API.Models.Entity.Moderator;

namespace MediCase.API.MappingProfiles
{
    public class ModeratorEntitiesMappingProfile : Profile
    {
        public ModeratorEntitiesMappingProfile()
        {
            CreateMap<Entities.Moderator.EntityTranslation, ModeratorEntityTranslationDto>()
                .ForSourceMember(src => src.EntityTranslationFiles, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Lang, opt => opt.DoNotValidate());
            CreateMap<ModeratorEntityTranslationDto, EntityTranslation>();


            CreateMap<Entity, ModeratorEntityDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumChildren, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.EntitiesGraphDatumParents, opt => opt.DoNotValidate());
            CreateMap<ModeratorEntityDto, Entities.Content.Entity>();

            CreateMap<EntityLanguage, ModeratorEntityLanguageDto>()
                .ForSourceMember(src => src.EntityTranslations, opt => opt.DoNotValidate());
            CreateMap<ModeratorEntityLanguageDto, Entities.Content.EntityLanguage>();

            CreateMap<EntityType, ModeratorEntityTypeDto>()
                .ForSourceMember(src => src.Entities, opt => opt.DoNotValidate());
            CreateMap<ModeratorEntityTypeDto, Entities.Content.EntityType>();

            CreateMap<EntityTranslationFile, ModeratorEntityTranslationFileDto>()
                .ForSourceMember(src => src.Translation, opt => opt.DoNotValidate());
            CreateMap<ModeratorEntityTranslationFileDto, Entities.Content.EntityTranslationFile>();

            CreateMap<EntitiesGraphDatum, ModeratorEntityGraphDataDto>()
                .ForSourceMember(src => src.Child, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Parent, opt => opt.DoNotValidate());
            CreateMap<ModeratorEntityGraphDataDto, Entities.Content.EntitiesGraphDatum>();
        }
    }
}
