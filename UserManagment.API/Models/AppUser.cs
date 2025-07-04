using System.ComponentModel.DataAnnotations;
using UserManagment.API.Enums;

namespace UserManagment.API.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public UserRoleEnum Role { get; set; } =  UserRoleEnum.User;
    }
}
