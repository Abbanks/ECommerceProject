using ECommerceProject.Services.ShoppingCartAPI.Models.Dto;

namespace ECommerceProject.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
