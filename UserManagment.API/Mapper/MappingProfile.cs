using AutoMapper;
using Project.Common.Dtos.User;
using UserManagment.API.Models;

namespace UserManagment.API.Mapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<AppUser, UserDto>();
        }
    }
}
