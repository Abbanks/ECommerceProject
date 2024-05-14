using AutoMapper;
using ECommerceProject.Services.CouponAPI.Models;
using ECommerceProject.Services.CouponAPI.Models.Dto;

namespace ECommerceProject.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            MapperConfiguration mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Coupon, CouponDto>().ReverseMap();
                cfg.CreateMap<Coupon, CouponDtoResponse>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
