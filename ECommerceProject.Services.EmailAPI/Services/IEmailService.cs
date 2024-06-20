using ECommerceProject.Services.EmailAPI.Models.Dto;

namespace ECommerceProject.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task RegisterUserEmailAndLog(string email);
       // Task LogOrderPlaced(RewardsMessage rewardsDto);
    }
}
