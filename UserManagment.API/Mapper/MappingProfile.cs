using AutoMapper;
using UserManagment.API.Dtos;
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
