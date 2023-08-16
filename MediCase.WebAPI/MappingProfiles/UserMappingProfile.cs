using AutoMapper;
using MediCase.WebAPI.Entities.Admin;
using MediCase.WebAPI.Models;
using MediCase.WebAPI.Models.Account;
using MediCase.WebAPI.Models.Group;
using MediCase.WebAPI.Models.User;

namespace MediCase.WebAPI.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, GetUserDto>()
                .ForMember(m => m.Roles, c => c.MapFrom<UserRolesResolver>());
            CreateMap<User, GetFullUserDto>()
                .ForMember(m => m.Roles, c => c.MapFrom<FullUserRolesResolver>());
            CreateMap<User, GetAccountDto>();
        }
    }
}
