using Microsoft.AspNetCore.Identity;

namespace ECommerceProject.Services.AuthAPI.Models
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; } = "";

    }
}
