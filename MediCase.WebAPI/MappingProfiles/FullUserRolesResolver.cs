using AutoMapper;
using MediCase.WebAPI.Entities.Admin;
using MediCase.WebAPI.Models.User;

namespace MediCase.WebAPI.MappingProfiles
{
    public class FullUserRolesResolver : IValueResolver<User, GetFullUserDto, List<string>>
    {
        public List<string> Resolve(User source, GetFullUserDto destination, List<string> destMember, ResolutionContext context)
        {
            List<string> roles = new List<string>();

            if (source.Groups != null)
            {
                foreach (var group in source.Groups)
                {
                    if (group.IsAdmin && !roles.Contains("Admin"))
                    {
                        roles.Add("Admin");
                    }
                    if (group.IsModerator && !roles.Contains("Moderator"))
                    {
                        roles.Add("Moderator");
                    }
                    if (group.IsUser && !roles.Contains("User"))
                    {
                        roles.Add("User");
                    }
                }
            }

            return roles;
        }
    }
}