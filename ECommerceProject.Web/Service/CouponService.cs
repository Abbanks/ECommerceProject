using ECommerceProject.Web.Models;

using ECommerceProject.Web.Service.IService;
using ECommerceProject.Web.Utility;

namespace ECommerceProject.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateCouponAsync(CouponDtoResponse couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = couponDto,
                Url = StaticDetail.CouponAPIBase + "/api/coupon/create-coupon/"
            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetail.ApiType.DELETE,
                Url = StaticDetail.CouponAPIBase + "/api/coupon/delete/" + id
            });
        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.CouponAPIBase + "/api/coupon/all"
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.CouponAPIBase + "/api/coupon/couponCode/" + couponCode
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.CouponAPIBase + "/api/coupon/couponId/" + id
            });
        }


        public async Task<ResponseDto?> UpdateCouponAsync(CouponDtoResponse couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetail.ApiType.PUT,
                Data = couponDto,
                Url = StaticDetail.CouponAPIBase + "/api/coupon/update-coupon/"
			});
        }
    }
}
