using ECommerceProject.Services.CouponAPI.Data;
using ECommerceProject.Services.CouponAPI.Models;
using ECommerceProject.Services.CouponAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace ECommerceProject.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;

        public CouponRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Coupon coupon)
        {
            await _context.Coupons.AddAsync(coupon);
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
            return await _context.Coupons.FindAsync(couponId);
        }

        public async Task<bool> UpdateAsync(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int couponId)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(i => i.CouponId == couponId);
            if (coupon == null)
            {
                return false;
            }
            _context.Coupons.Remove(coupon);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
