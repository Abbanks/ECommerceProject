using AutoMapper;
using ECommerceProject.Services.ProductAPI.Models.Dto;
using ECommerceProject.Services.ProductAPI.Models;

namespace ECommerceProject.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            MapperConfiguration mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>().ReverseMap();
                cfg.CreateMap<Product, ProductDtoResponse>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
