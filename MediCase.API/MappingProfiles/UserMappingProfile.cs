using AutoMapper;
using MediCase.API.Entities.Admin;
using MediCase.API.Models.Account;
using MediCase.API.Models.User;
using MediCase.API.Models;
using MediCase.API.Models.Group;

namespace MediCase.API.MappingProfiles
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
