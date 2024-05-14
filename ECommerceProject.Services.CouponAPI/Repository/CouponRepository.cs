using ECommerceProject.Services.CouponAPI.Data;
using ECommerceProject.Services.CouponAPI.Models;
using ECommerceProject.Services.CouponAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProject.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        public AppDbContext _context;

        public CouponRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Coupon coupon)
        {
            await _context.AddAsync(coupon);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return await _context.Coupons.ToListAsync();
        }

        public async Task<Coupon> GetByCodeAsync(string couponCode)
        {
            return await _context.Coupons.FirstOrDefaultAsync(x => x.CouponCode.ToLower() == couponCode.ToLower());
        }

        public async Task<Coupon> GetByIdAsync(int couponId)
        {
            return await _context.FindAsync<Coupon>(couponId);

        }

        public async Task<bool> UpdateAsync(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int couponId)
        {
            var coupon = _context.Coupons.FirstOrDefault(i => i.CouponId == couponId);
            _context.Coupons.Remove(coupon);
            return await _context.SaveChangesAsync() > 0;
        }



    }
}
