using ECommerceProject.Services.AuthAPI.Models;

namespace ECommerceProject.Services.AuthAPI.Service.IService
{
    public interface IJwtTokenService
    {
        string GenerateToken(AppUser user, IEnumerable<string> roles);
    }
}
