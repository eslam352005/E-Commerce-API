using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.DTOs;
using Shared.Model_Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        
        [HttpGet]
        [Cache(300)]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams productQuery)
        {
            
            var products = await _serviceManager.productService.GetAllProductsAsync(productQuery);
            return Ok(products);
        }
        [ProducesResponseType(typeof(ProductDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorToReturn), StatusCodes.Status404NotFound)]
        [HttpGet("{id:int}")]

        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _serviceManager.productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpGet("brands")]

        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        {
            var brands = await _serviceManager.productService.GetAllBrandsAsync();
            return Ok(brands);

        }

        [HttpGet("types")]

        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        {
            var types = await _serviceManager.productService.GetAllTypesAsync();
            return Ok(types);

        }

    }
}
