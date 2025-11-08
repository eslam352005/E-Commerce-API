using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(string key);
        Task<CartDto> CreateOrUpdateCartAsync(CartDto cart);
        Task<bool> DeleteCartAsync(string key);
    }
}
