using Shared;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        public Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams productQuery);
        public Task<ProductDto> GetProductByIdAsync(int id);
        public Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
        public Task<IEnumerable<TypeDto>> GetAllTypesAsync();

    }
}
