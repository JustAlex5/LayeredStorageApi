using Project.Common.Dtos.Auth;
using Project.Common.Dtos.User;
using Project.Common.Enums;
using Project.Common.Models;

namespace UserManagment.API.BL
{
    public interface IUserServices
    {
        public  Task<ResponseModel<UserDto>> LoginAsync(LoginDto login);
        public Task<ResponseModel<UserDto>> RegisterAsync(LoginDto newUser);
        public Task<ResponseModel<UserDto>> UpdateRole(int id, UserRoleEnum role);



    }
}
