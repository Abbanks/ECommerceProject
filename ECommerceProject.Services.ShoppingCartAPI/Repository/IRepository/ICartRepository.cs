namespace ECommerceProject.Services.ShoppingCartAPI.Repository.IRepository
{
    public interface ICartRepository
    {
        Task<bool> AddAsync<T>(T entity) where T : class;
        Task<bool> UpdateAsync<T>(T entity) where T : class;
        Task<bool> DeleteAsync<T>(T entity) where T : class;
        Task<T?> GetByIdAsync<T>(int id) where T : class;
        IQueryable<T> GetAllAsync<T>() where T : class;
    }
}
