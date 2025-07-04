using UserManagment.API.Enums;

namespace UserManagment.API.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public UserRoleEnum Role { get; set; }
    }
}
