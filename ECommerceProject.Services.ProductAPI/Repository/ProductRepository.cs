
using ECommerceProject.Services.ProductAPI.Data;
using ECommerceProject.Services.ProductAPI.Models;
using ECommerceProject.Services.ProductAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProject.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AddAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int productId)
        {
            var product = await _db.Products.FirstOrDefaultAsync(u => u.ProductId == productId);
            if (product == null)
            {
                return false;
            }
            _db.Products.Remove(product);
            return await Save();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int productId)
        {
            return await _db.Products.FirstOrDefaultAsync(u => u.ProductId == productId);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            _db.Products.Update(product);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }
    }
}
