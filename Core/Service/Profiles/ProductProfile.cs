using AutoMapper;
using AutoMapper.Execution;
using Domain.Models.Products;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Profiles
{
    public class ProductProfile : Profile
    {

        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
            .ForMember(d => d.BrandName, options => options.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.TypeName, options => options.MapFrom(s => s.productType.Name))
            .ForMember(d => d.PictureURL, options => options.MapFrom<URLResolver>());

            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType, TypeDto>();
        }

    }
}
