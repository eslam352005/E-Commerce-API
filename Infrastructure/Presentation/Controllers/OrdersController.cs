using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController(IServiceManager _serviceManager) : ControllerBase
    {
        
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _serviceManager.orderService.CreateOrderAsync(orderDto, email!);

            return Ok(order);
        }
        [AllowAnonymous]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
          var methods = await _serviceManager.orderService.GetDeliveryMethodsAsync();
            return Ok(methods);
        } [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var orders = await _serviceManager.orderService.GetAllOrdersAsync(email);
            return Ok(orders);
        }

       
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrders(Guid id)
        {
           
            var order = await _serviceManager.orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }

    }
}
