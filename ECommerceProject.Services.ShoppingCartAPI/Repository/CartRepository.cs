
using ECommerceProject.Services.ShoppingCartAPI.Data;
using ECommerceProject.Services.ShoppingCartAPI.Repository.IRepository;

namespace ECommerceProject.Services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _db;

        public CartRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AddAsync<T>(T entity) where T : class
        {
            await _db.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> DeleteAsync<T>(T entity) where T : class
        {
            _db.Remove(entity);
            return await Save();
        }

        public IQueryable<T> GetAllAsync<T>() where T : class
        {
            return _db.Set<T>();
        }

        public async Task<T?> GetByIdAsync<T>(int id) where T : class
        {
            return await _db.FindAsync<T>(id);
        }

        public async Task<bool> UpdateAsync<T>(T entity) where T : class
        {
            _db.Update(entity);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }
    }
}
