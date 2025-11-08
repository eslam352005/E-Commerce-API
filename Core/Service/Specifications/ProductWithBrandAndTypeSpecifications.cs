using Domain.Models.Products;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecification<Product,int>
    {
        // GetAll
        public ProductWithBrandAndTypeSpecifications(ProductQueryParams productQuery)
            :base(p=> (!productQuery.brandId.HasValue || p.BrandId == productQuery.brandId) &&
                      (!productQuery.typeId.HasValue || p.TypeId == productQuery.typeId) &&
                      (string.IsNullOrWhiteSpace(productQuery.SearchValue) || p.Name.ToUpper().Contains(productQuery.SearchValue.ToUpper())))
        {
            AddInclude(p => p.productType);
            AddInclude(p => p.ProductBrand);

            switch (productQuery.sortingOption)
            {
                case ProductSortingOption.NameAsc:
                    AddOrder(p=>  p.Name);
                    break;
                case ProductSortingOption.NameDesc:
                    AddOrderDesc(p => p.Name);
                    break;
                case ProductSortingOption.PriceAsc:
                    AddOrder(p => p.Price);
                    break;
                case ProductSortingOption.PriceDesc:
                    AddOrderDesc(p => p.Price);
                    break;
                    default:
                    break;

            }
            Paginate(productQuery.PageIndex, productQuery.PageSize);

        }

        // GetById
        public ProductWithBrandAndTypeSpecifications(int id) : base(p=> p.Id == id)
        {
            AddInclude(p => p.productType);
            AddInclude(p => p.ProductBrand);
        }
    }
}
