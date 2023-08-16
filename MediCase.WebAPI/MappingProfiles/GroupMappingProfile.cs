using AutoMapper;
using MediCase.WebAPI.Entities.Admin;
using MediCase.WebAPI.Models.Group;

namespace MediCase.WebAPI.MappingProfiles
{
    public class GroupMappingProfile : Profile
    {
        public GroupMappingProfile() 
        {
            CreateMap<GroupDto, Group>();
            CreateMap<Group, GetGroupDto>()
                .ForMember(m => m.UsersCount, c => c.MapFrom(s => s.Users.Count));
            CreateMap<Group, GetFullGroupDto>()
                .ForMember(m => m.Users, c => c.MapFrom(s => s.Users))
                .ForMember(m => m.UsersCount, c => c.MapFrom(s => s.Users.Count));
        }
    }
}
