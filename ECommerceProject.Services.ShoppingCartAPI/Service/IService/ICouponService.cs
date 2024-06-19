using ECommerceProject.Services.ShoppingCartAPI.Models.Dto;

namespace ECommerceProject.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
