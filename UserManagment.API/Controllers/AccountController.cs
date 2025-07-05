using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Dtos.Auth;
using Project.Common.Dtos.User;
using Project.Common.Enums;
using Project.Common.Models;
using UserManagment.API.Services.Interfaces;

namespace UserManagment.API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly IUserServices _userServices;

        public AccountController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("register")]
        [AllowAnonymous]

        public async Task<ActionResult<ResponseModel<UserDto>>> Register([FromBody] LoginDto userDto)
        {
            var response = await _userServices.RegisterAsync(userDto);
            return StatusCode(response.StatusCode, response);

        }
        [HttpPost ("login")]
        [AllowAnonymous]

        public async Task<ActionResult<ResponseModel<UserDto>>> Login([FromBody] LoginDto login)
        {
            var res = await _userServices.LoginAsync(login);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut("update-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRoleDto roleDto)
        {
            var  res = await _userServices.UpdateRole(roleDto.Id, roleDto.UserRole);
            return StatusCode(res.StatusCode, res);
        }
    }
}
