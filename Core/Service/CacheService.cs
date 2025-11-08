using Domain.Contracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service
{
    public class CacheService(ICacheRepo _repo) : ICacheService
    {
        public async Task<string?> GetCacheAsync(string cacheKey)
        {
           return await _repo.GetAsync(cacheKey);
        }

        public async Task SetCacheAsync(string cacheKey, object cacheValue, TimeSpan TimeToLive)
        {
            var value = JsonSerializer.Serialize(cacheValue);

            await _repo.SetAsync(cacheKey, value, TimeToLive);
        }
    }
}
