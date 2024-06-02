using ECommerceProject.Services.AuthAPI.Models.Dto;
using ECommerceProject.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceProject.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new ResponseDto();
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(string email, string role)
        {
            var assignRoleSuccessful = await _authService.AssignRole(email, role);

            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var result = await _authService.Register(model);
            if (!string.IsNullOrEmpty(result))
            {
                _response.IsSuccess = false;
                _response.Message = result;
                return BadRequest(_response);
            }
            _response.Message = "Registration successful";
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);

            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid Credentials";
                return BadRequest(_response);
            }

            _response.Result = loginResponse;
            _response.Message = "Login successful";
            return Ok(_response);
        }
    }
}
