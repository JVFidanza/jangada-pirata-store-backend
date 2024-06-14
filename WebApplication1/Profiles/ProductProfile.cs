using AutoMapper;
using WebApplication1.Data.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<Product, UpdateProductDto>();
    }
}