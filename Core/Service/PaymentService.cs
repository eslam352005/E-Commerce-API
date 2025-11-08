using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Carts;
using Domain.Models.Orders;
using Microsoft.Extensions.Configuration;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DTOs;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PaymentService(IConfiguration configuration,
                                ICartRepo _cartRepo,
                                IUnitOfWork _unitOfWork,
                                IMapper mapper) : IPaymentService
    {
        public async Task<CartDto> CreateOrUpdatePaymentIntentAsync(string cartId)
        {
            StripeConfiguration.ApiKey = configuration.GetSection("Stripe")["SecretKey"];

            var cart = await _cartRepo.GetAsync(cartId);
            if (cart == null)
            {
                throw new CartNotFoundException(cartId);
            }

            foreach (var item in cart.Items)
            {
                var product = await _unitOfWork.GetRepo<Domain.Models.Products.Product, int>().GetByIdAsync(item.Id);
                if(product == null)
                {
                    throw new ProductNotFound(item.Id);
                }
                item.Price = product.Price;
            }

            if (cart.DeliveryMethodId == null)
            {
                throw new ArgumentNullException();
            }

            var deliveryMethod = await _unitOfWork.GetRepo<DeliveryMethod, int>().GetByIdAsync(cart.DeliveryMethodId.Value);

            if(deliveryMethod == null)
            {
                throw new DeliveryMethodNotFound(cart.DeliveryMethodId.Value);
            }
            cart.ShippingPrice = deliveryMethod.Price;

            var amount = (long) (cart.Items.Sum(i => i.Price * i.Quantity) + cart.ShippingPrice) * 100;

            var service = new PaymentIntentService();

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "AED",
                    PaymentMethodTypes = ["card"]

                };

                var paymentIntent = await service.CreateAsync(options);
                cart.PaymentIntentId = paymentIntent.Id;
                cart.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = amount
                };
               await service.UpdateAsync(cart.PaymentIntentId, options);
            }

            await _cartRepo.CreateOrUpdateAsync(cart);
            return mapper.Map<Cart,CartDto>(cart);
        }


        public async Task UpdatePaymentStatusAsync(string request, string stripeHeader)
        {
            var endpointSecret = configuration.GetSection("Stripe")["EndPointSecret"];
            var stripeEvent = EventUtility.ConstructEvent(request, stripeHeader, endpointSecret);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentPaymentFailed:
                    await UpdatePaymentFailedAsync(paymentIntent.Id);

                    break;
                case EventTypes.PaymentIntentSucceeded:
                    await UpdatePaymentReceivedAsync(paymentIntent.Id);

                    break;

                default:
                    Console.WriteLine($"Unhandled Stripe event type {stripeEvent.Type}");
                    break;
            }
        }


        private async Task UpdatePaymentReceivedAsync(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepo<Order, Guid>()
                .GetByIdAsync(new OrderToPaymentIntentSpec(paymentIntentId));
          

            order.OrderStatus = OrderStatus.PaymentReceived;

            _unitOfWork.GetRepo<Order,Guid>().Update(order);
           await _unitOfWork.SaveChangesAsync();
        }

        private async Task UpdatePaymentFailedAsync(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepo<Order, Guid>()
                .GetByIdAsync(new OrderToPaymentIntentSpec(paymentIntentId));


            order.OrderStatus = OrderStatus.PaymentFailed;

            _unitOfWork.GetRepo<Order, Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
