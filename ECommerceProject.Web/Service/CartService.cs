using ECommerceProject.Web.Models;
using ECommerceProject.Web.Service.IService;
using ECommerceProject.Web.Utility;

namespace ECommerceProject.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = cartDto,
                Url = StaticDetail.ShoppingCartAPIBase + "/api/cart/apply_coupon"
            });
        }

        public async Task<ResponseDto?> EmailCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = cartDto,
                Url = StaticDetail.ShoppingCartAPIBase + "/api/cart/email_cart_request"
            });
        }

        public async Task<ResponseDto?> GetCartByUserIdAsnyc(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.ShoppingCartAPIBase + "/api/cart/get_cart/" + userId
            });
        }


        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = cartDetailsId,
                Url = StaticDetail.ShoppingCartAPIBase + "/api/cart/remove_cart"
            });
        }


        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = cartDto,
                Url = StaticDetail.ShoppingCartAPIBase + "/api/cart/upsert_cart"
            });
        }
    }
}
