using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Carts;
using ServiceAbstraction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CartService(ICartRepo _repo , IMapper mapper) : ICartService
    {
        public async Task<CartDto> CreateOrUpdateCartAsync(CartDto cart)
        {
            var Cart = mapper.Map<CartDto, Cart>(cart);
            var CreatedOrUpdated = await _repo.CreateOrUpdateAsync(Cart);
            if (CreatedOrUpdated != null)
            {
                return await GetCartAsync(cart.Id);
            }
            else
            {
                throw new Exception("Can't create or update cart right now , Try Later");
            }
        }

        public async Task<bool> DeleteCartAsync(string key)
        {
           return await _repo.DeleteAsync(key);
        }

        public async Task<CartDto> GetCartAsync(string key)
        {
           var cart = await _repo.GetAsync(key);
            if (cart == null)
            {
                throw  new CartNotFoundException(key);
            }
            return mapper.Map<Cart,CartDto>(cart);
        }
    }
}
