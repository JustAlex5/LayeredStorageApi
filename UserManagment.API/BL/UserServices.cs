using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Common.Dtos.Auth;
using Project.Common.Dtos.User;
using Project.Common.Enums;
using Project.Common.Models;
using Project.Common.Utils;
using System.Security.Cryptography;
using System.Text;
using UserManagment.API.DbData;
using UserManagment.API.Models;

namespace UserManagment.API.BL
{
    public class UserServices : IUserServices
    {
        private readonly ITokenService _tokenService;
        private readonly DataContext _context;
        private readonly ILogger<UserServices> _logger;
        private readonly IMapper _mapper;
        private const string Action = nameof(UserServices);

        public UserServices(ITokenService tokenService, DataContext context, ILogger<UserServices> logger, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseModel<UserDto>> LoginAsync(LoginDto login)
        {
            try
            {
                var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == login.Username);
                if (user == null)
                {
                    _logger.LogWarning("{Action}: Login failed for username: {Username} - user not found", Action,login.Username);
                    return ResponseFactory.Error<UserDto>("UserName or Password are incorrect", 401);
                }

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Token = _tokenService.CreateToken(userDto);

                return ResponseFactory.Success(userDto, "Login successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Error occurred during login for username: {Username}", Action,login.Username);
                return ResponseFactory.Error<UserDto>("An error occurred during login.");
            }
        }

        public async Task<ResponseModel<UserDto>> RegisterAsync(LoginDto newUser)
        {
            try
            {
                if (await _context.AppUsers.AnyAsync(x => x.UserName == newUser.Username))
                {
                    _logger.LogWarning("{Action}: Register failed: username already taken - {Username}" ,Action, newUser.Username);
                    return ResponseFactory.Error<UserDto>("This user name has been taken", StatusCodes.Status400BadRequest);
                }

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Error occurred while registering user: {Username}", Action,newUser.Username);
                return ResponseFactory.Error<UserDto>("An error occurred during registration.");
            }
        }

        public async Task<ResponseModel<UserDto>> UpdateRole(int id, UserRoleEnum role)
        {
            try
            {
                var currentUser = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
                if (currentUser == null)
                {
                    _logger.LogWarning("{Action}: UpdateRole failed: user with ID {UserId} not found", Action,id);
                    return ResponseFactory.Error<UserDto>("User with the given ID does not exist.");
                }

                currentUser.Role = role;
                _context.AppUsers.Update(currentUser);
                await _context.SaveChangesAsync();

                var userDto = _mapper.Map<UserDto>(currentUser);
                return ResponseFactory.Success(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Action}: Error occurred while updating role for user ID: {UserId}", Action,id);
                return ResponseFactory.Error<UserDto>("An error occurred while updating the user role.");
            }
        }
    }
}
