using Project.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Dtos.User
{
    public class UpdateUserRoleDto
    {
        public int Id { get; set; }
        public UserRoleEnum UserRole { get; set; }
    }
}
