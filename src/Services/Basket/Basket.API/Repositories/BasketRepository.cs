using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrWhiteSpace(basket))
                return null;

            var ret = JsonConvert.DeserializeObject<ShoppingCart>(basket);
            return ret;
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            string basketStr = JsonConvert.SerializeObject(basket);
            await _redisCache.SetStringAsync(basket.UserName, basketStr);
            return await GetBasket(basket.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }

    }
}
