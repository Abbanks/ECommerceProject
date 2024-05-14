using ECommerceProject.Services.CouponAPI.Models;

namespace ECommerceProject.Services.CouponAPI.Repository.Interface
{
    public interface ICouponRepository
    {

        Task<bool> AddAsync(Coupon coupon);
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task<Coupon> GetByIdAsync(int couponId);
        Task<Coupon> GetByCodeAsync(string couponCode);
        Task<bool> UpdateAsync(Coupon coupon);
        Task<bool> DeleteAsync(int id);
    }
}
