using Domain.Contracts;
using Domain.Models.Carts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Repos
{
    public class CartRepo(IConnectionMultiplexer connection) : ICartRepo
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<Cart?> CreateOrUpdateAsync(Cart cart, TimeSpan? TimeToLive = null)
        {
          var json = JsonSerializer.Serialize(cart);
          var isUpdatedOrCreated =  await _database.StringSetAsync(cart.Id,json,TimeToLive ?? TimeSpan.FromDays(30));
            if (isUpdatedOrCreated)
            {
                return await GetAsync(cart.Id);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(string key)
        {
           return await _database.KeyDeleteAsync(key);
        }

        public async Task<Cart?> GetAsync(string key)
        {
           var cart = await _database.StringGetAsync(key);
            if (cart.IsNullOrEmpty)
            {
                return null;
            }
            return  JsonSerializer.Deserialize<Cart>(cart!);
        }
    }
}
