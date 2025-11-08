using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Products;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepo<ProductBrand,int>().GetAllAsync();
            return mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(brands);
        }

        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams productQuery)
        {
            var specifications = new ProductWithBrandAndTypeSpecifications(productQuery);
            var products = await _unitOfWork.GetRepo<Product, int>().GetAllAsync(specifications);
            var ProductsDto =  mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            var ProductsCount = new ProductCountSpecification(productQuery);
            var TotalCount = await _unitOfWork.GetRepo<Product,int>().CountAsync(ProductsCount);
            return new PaginatedResult<ProductDto>(ProductsDto.Count(),productQuery.PageIndex, TotalCount, ProductsDto);
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepo<ProductType, int>().GetAllAsync();
            return mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(Types);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var specification = new ProductWithBrandAndTypeSpecifications(id);
           var product = await _unitOfWork.GetRepo<Product,int>().GetByIdAsync(specification);
            if(product == null)
            {
                throw new ProductNotFound(id);
            }
            return mapper.Map<Product, ProductDto>(product);
        }
    }
}
