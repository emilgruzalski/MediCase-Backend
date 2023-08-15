using AutoMapper;
using MediCase.WebAPI.Models.Entity;

namespace MediCase.WebAPI.MapperProfiles
{
    public class EntityTranslationFileProfile : Profile
    {
        public EntityTranslationFileProfile() 
        {
            CreateMap<Entities.Moderator.EntityTranslationFile, EntityTranslationFileDto>()
                .ForSourceMember(src => src.Translation, opt => opt.DoNotValidate())
                .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePathHashed));
            CreateMap<Entities.Content.EntityTranslationFile, EntityTranslationFileDto>()
                .ForSourceMember(src => src.Translation, opt => opt.DoNotValidate())
                .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePathHashed));
        }
    }
}
