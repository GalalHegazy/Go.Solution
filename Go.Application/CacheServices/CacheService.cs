using Go.Core.Services.Contract;
using StackExchange.Redis;
using System.Text.Json;

namespace Go.Application.CacheServices
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        public CacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            if (response is null) return;

            var serializerOption = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var serializeredResponse = JsonSerializer.Serialize(response, serializerOption); // From String To Json with CamelCase Name

            await _database.StringSetAsync(key, serializeredResponse, timeToLive);
        }
        public async Task<string?> GetCacheResponseAsync(string key)
        {
            var response = await _database.StringGetAsync(key);

            if(response.IsNullOrEmpty) return null;   

            return response;
        }
    }
}
