using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Project.Common.Models;
using Project.Common.Utils;
using System.Security.Cryptography;
using System.Text;
using UserManagment.API.DbData;
using UserManagment.API.Dtos;
using UserManagment.API.Enums;
using UserManagment.API.Models;

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

        public async Task<ResponseModel<UserDto>> RegisterAsync(LoginDto newUser)
        {
            if (await _context.AppUsers.AnyAsync(x => x.UserName == newUser.Username)) 
                return ResponseFactory.Error<UserDto>("This user name has been taken", StatusCodes.Status400BadRequest);

            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName = newUser.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newUser.Password)),
                PasswordSalt = hmac.Key
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = _tokenService.CreateToken(userDto);
            return ResponseFactory.Success(userDto);

        }

        public async Task<ResponseModel<UserDto>> UpdateRoll(int id , UserRoleEnum role )
        {
            var currentUser = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
            if (currentUser == null)
                return ResponseFactory.Error<UserDto>("User with the given ID does not exist.");

            currentUser.Role = role;
            _context.AppUsers.Update(currentUser);
            await _context.SaveChangesAsync();
            var userDto = _mapper.Map<UserDto>(currentUser);
            return ResponseFactory.Success(userDto);

        }
    }

}
