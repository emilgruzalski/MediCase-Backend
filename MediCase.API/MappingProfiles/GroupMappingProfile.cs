using AutoMapper;
using MediCase.API.Entities.Admin;
using MediCase.API.Models.Group;

namespace MediCase.API.MappingProfiles
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
