using Project.Common.Enums;

namespace Project.Common.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public UserRoleEnum Role { get; set; }
    }
}
