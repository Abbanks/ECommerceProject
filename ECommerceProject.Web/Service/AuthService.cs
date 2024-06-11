using ECommerceProject.Web.Models;
using ECommerceProject.Web.Service.IService;
using ECommerceProject.Web.Utility;
using System.Data;

namespace ECommerceProject.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> AssignRoleAsync(string email, string role)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = new { Email = email, Role = role },
                Url = StaticDetail.AuthAPIBase + "/api/auth/assign-role"
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto model)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = model,
                Url = StaticDetail.AuthAPIBase + "/api/auth/login"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> RegisterUserAsync(RegistrationRequestDto model)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = model,
                Url = StaticDetail.AuthAPIBase + "/api/auth/register"
            }, withBearer: false);
        }
    }
}
