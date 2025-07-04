using Project.Common.Models;
using UserManagment.API.Dtos;
using UserManagment.API.Enums;

namespace UserManagment.API.BL
{
    public interface IUserServices
    {
        public  Task<ResponseModel<UserDto>> LoginAsync(LoginDto login);
        public Task<ResponseModel<UserDto>> RegisterAsync(LoginDto newUser);
        public Task<ResponseModel<UserDto>> UpdateRoll(int id, UserRoleEnum role);



    }
}
