using Go.Core.Entities.Basket;
using Go.Core.Repositories.Contract;
using StackExchange.Redis;
using System.Text.Json;

namespace Go.Infrastructure.Basket_Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var basket = await _database.StringGetAsync(BasketId);

            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }
        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            var createdOrupdated = await _database.StringSetAsync(Basket.Id, JsonSerializer.Serialize(Basket),TimeSpan.FromDays(30)); // Retern Flag

            if(!createdOrupdated) return null;   

            return await GetBasketAsync(Basket.Id);
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
           return await _database.KeyDeleteAsync(BasketId);
        }

    }
}
