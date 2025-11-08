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
    public class PaymentController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("{cartId}")]
        public async Task<ActionResult<CartDto>> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await _serviceManager.paymentService.CreateOrUpdatePaymentIntentAsync(cartId);
            return Ok(cart);
        }
        [HttpPost("WebHook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            await _serviceManager.paymentService.UpdatePaymentStatusAsync(json, HttpContext.Request.Headers["Stripe-Signature"]!);
            return new EmptyResult();
        }

    }
}
