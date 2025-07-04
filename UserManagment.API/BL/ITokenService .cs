using UserManagment.API.Dtos;

namespace UserManagment.API.BL
{
    public interface ITokenService
    {
        public string CreateToken(UserDto user);

    }
}
