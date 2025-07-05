using Project.Common.Dtos.User;

namespace UserManagment.API.BL
{
    public interface ITokenService
    {
        public string CreateToken(UserDto user);

    }
}
