using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart(string key)
        {
            var cart = await _serviceManager.cartService.GetCartAsync(key);
            return Ok(cart);
        }
        [HttpPost]
        public async Task<ActionResult<CartDto>> CreateOrUpdateCart(CartDto cart)
        {
            var Cart = await _serviceManager.cartService.CreateOrUpdateCartAsync(cart);
            return Ok(Cart);
        }
        [HttpDelete("{key}")]
        public async Task<ActionResult<bool>> DeleteCart(string key)
        {
            var Cart = await _serviceManager.cartService.DeleteCartAsync(key);
            return Ok(Cart);
        }

    }
}
