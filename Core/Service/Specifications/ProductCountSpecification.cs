using Domain.Models.Products;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    internal class ProductCountSpecification : BaseSpecification<Product, int> 
    {

        public ProductCountSpecification(ProductQueryParams productQuery) 
            : base(p => (!productQuery.brandId.HasValue || p.BrandId == productQuery.brandId) &&
                      (!productQuery.typeId.HasValue || p.TypeId == productQuery.typeId) &&
                      (string.IsNullOrWhiteSpace(productQuery.SearchValue) || p.Name.ToUpper().Contains(productQuery.SearchValue.ToUpper())))
        {
            
        }
    }
}
