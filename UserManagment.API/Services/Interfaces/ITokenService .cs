using Project.Common.Dtos.User;

namespace UserManagment.API.Services.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(UserDto user);

    }
}
