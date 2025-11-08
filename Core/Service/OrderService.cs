using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Orders;
using Domain.Models.Products;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OrderService(IMapper _mapper , ICartRepo _cartRepo , IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email)
        {
          
            var OrderAddress = _mapper.Map<AddressDto,OrderAddress>(orderDto.Address);

            var cart = await _cartRepo.GetAsync(orderDto.CartId);
            if (cart == null)
            {
                throw new CartNotFoundException(orderDto.CartId);
            }

            var currentOrderSpecs = new OrderToPaymentIntentSpec(cart.PaymentIntentId!);
            var currentOrder = await _unitOfWork.GetRepo<Order, Guid>()
                .GetByIdAsync(currentOrderSpecs);

            if(currentOrder != null)
            {
                 _unitOfWork.GetRepo<Order, Guid>()
                .Remove(currentOrder);
            }


            List<OrderedItem> orderedItems = new List<OrderedItem>();
            var productRepo = _unitOfWork.GetRepo<Product, int>();

            foreach (var item in cart.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id);
                if(product == null) 
                {
                    throw new ProductNotFound(item.Id);
                }
                var OrderItem = new OrderedItem()
                {
                    Product = new ProductItemOrdered()
                    {
                        ProductId = product.Id,
                        PictureUrl = product.PictureUrl,
                        ProductName = product.Name,
                    },
                    Price = product.Price,
                    Quantity = item.Quantity,
                };
                orderedItems.Add(OrderItem);
            }


            var DeliveryMethod = await _unitOfWork.GetRepo<DeliveryMethod,int>().GetByIdAsync(orderDto.DeliveryMethodId);
            if( DeliveryMethod == null)
            {
                throw new DeliveryMethodNotFound(orderDto.DeliveryMethodId);
            }

            var SubTotal = orderedItems.Sum(oi=> oi.Price * oi.Quantity );

            var order = new Order(email, OrderAddress, DeliveryMethod, orderedItems, SubTotal,cart.PaymentIntentId!);
            await _unitOfWork.GetRepo<Order,Guid>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Order,OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string email)
        {
            var spec = new OrderSpecification(email);
            var orders = await _unitOfWork.GetRepo<Order, Guid>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>(orders);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
           var methods = await _unitOfWork.GetRepo<DeliveryMethod,int>().GetAllAsync();
           return _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDto>>(methods);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecification(id);
            var order = await _unitOfWork.GetRepo<Order,Guid>().GetByIdAsync(spec);
            if(order == null)
            {
                throw new OrderNotFound(id);
            }

            return _mapper.Map<Order, OrderToReturnDto>(order);
        }
    }
}
