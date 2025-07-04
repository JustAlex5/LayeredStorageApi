using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Models;
using UserManagment.API.BL;
using UserManagment.API.Dtos;
using UserManagment.API.Enums;

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
        public async Task<IActionResult> UpdateUser([FromQuery] int id, [FromQuery]UserRoleEnum userRole)
        {
            var  res = await _userServices.UpdateRoll(id, userRole);
            return StatusCode(res.StatusCode, res);
        }
    }
}
