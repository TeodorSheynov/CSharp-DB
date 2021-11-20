using AutoMapper;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Import DTO-s
            this.CreateMap<ImportDtos.UserDto, User>();
            this.CreateMap<ImportDtos.ProductDto, Product>();
            this.CreateMap<ImportDtos.CategoryDto, Category>();
            this.CreateMap<ImportDtos.CategoryProductsDto, CategoryProduct>();
            //Export DTO-s
            this.CreateMap<Product, ExportDtos.ProductDto>()
                .ForMember(x=>x.Seller,
                    opt=>opt
                        .MapFrom(src=> $"{src.Seller.FirstName} {src.Seller.LastName}"));
            this.CreateMap<Category, ExportDtos.CategoryDto>();
        }
    }
}
