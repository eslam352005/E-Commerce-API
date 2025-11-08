using Domain.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICartRepo
    {
        Task<Cart?> GetAsync(string key);
        Task<Cart?> CreateOrUpdateAsync(Cart cart , TimeSpan? TimeToLive = null );
        Task<bool> DeleteAsync(string key);
    }
}
