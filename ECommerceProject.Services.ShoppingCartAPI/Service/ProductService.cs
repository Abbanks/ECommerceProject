using ECommerceProject.Services.ShoppingCartAPI.Models.Dto;
using ECommerceProject.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;
 

    public ProductService(IHttpClientFactory httpClientFactory )
    {
        _httpClientFactory = httpClientFactory;
         
    }

    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        var products = new List<ProductDto>();

        try
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync("/api/product/products");

            if (response.IsSuccessStatusCode)
            {
                var apiContent = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

                if (resp.IsSuccess)
                {
                    products = (List<ProductDto>)JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception as per your application's requirements
            Console.WriteLine($"Error fetching products: {ex.Message}");
        }

        return products;
    }
}
