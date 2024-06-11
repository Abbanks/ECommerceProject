using ECommerceProject.Web.Models;

namespace ECommerceProject.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> RegisterUserAsync(RegistrationRequestDto model);
        Task<ResponseDto?> LoginAsync(LoginRequestDto model);
        Task<ResponseDto?> AssignRoleAsync(string email, string role);
    }
}
