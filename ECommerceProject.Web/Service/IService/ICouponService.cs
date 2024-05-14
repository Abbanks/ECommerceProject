using ECommerceProject.Web.Models;

namespace ECommerceProject.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponAsync(string couponCode);
        Task<ResponseDto?> GetAllCouponsAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponAsync(CouponDtoResponse couponDto);
        Task<ResponseDto?> UpdateCouponAsync(CouponDtoResponse couponDto);
        Task<ResponseDto?> DeleteCouponAsync(int id);
    }
}
