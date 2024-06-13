using ECommerceProject.Services.ProductAPI.Models;

namespace ECommerceProject.Services.ProductAPI.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<bool> AddAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int productId);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}
