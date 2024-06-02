using ECommerceProject.Services.AuthAPI.Models;
using ECommerceProject.Services.AuthAPI.Models.Dto;
using ECommerceProject.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace ECommerceProject.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            AppUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

                if (result.Succeeded)
                {
                    var userToReturn = await _userManager.FindByEmailAsync(registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        Email = userToReturn.Email,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "SignUp Successful"; // Consider returning a meaningful message or user data if required
                }
                else
                {
                    return result.Errors.FirstOrDefault()?.Description;
                }
            }
            catch (Exception e)
            {
                // Log the exception (e.g., using a logging framework)
                return $"Error encountered: {e.Message}";
            }
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByNameAsync(loginRequestDto.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = ""
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenService.GenerateToken(user, roles);

            var userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };
        }

    }
}
