using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Common.Models;
using Project.Common.Utils;
using UserManagment.API.DbData;
using UserManagment.API.Dtos;

namespace UserManagment.API.BL
{
    public class UserServices : IUserServices
    {
        private readonly ITokenService _tokenService;
        private readonly DataContext _context;
        private readonly ILogger<UserServices> _logger;
        private readonly IMapper _mapper;

        public UserServices(ITokenService tokenService, DataContext context, ILogger<UserServices> logger, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseModel<UserDto>> LoginAsync(LoginDto login)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(x =>
            x.UserName == login.Username);
            if (user == null) return ResponseFactory.Error<UserDto>("UserName or Password are incorect", 401);

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = _tokenService.CreateToken(userDto);

            return ResponseFactory.Success(userDto, "Login successful");


        }
    }

}
