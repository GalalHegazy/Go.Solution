using Go.Core.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace Go.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSecound;

        public CachedAttribute(int TimeToLiveInSecound)
        {
            _timeToLiveInSecound = TimeToLiveInSecound;
        }

        public  async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1- Ask Clr For Create Obj from ICacheService (Explisitly)
            var _cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            // 2- Genrate CacheKey
            var cacheKey = GenerateCacheKey(context.HttpContext.Request);

            //  End Point has a Cache Response
            var response = await _cacheService.GetCacheResponseAsync(cacheKey);

            //  has a CachedResponse ?
            if (!string.IsNullOrEmpty(response))
            {
                var result = new ContentResult()
                {
                  Content = response,
                  ContentType = "application/json",
                  StatusCode = 200,

                };
                context.Result = result;

                return; // To Go Out from this method
            }


            // 3- Invoke To Execute the next action filter or the action it self , and The EndPoint Has't CacheResponse
             var actionExecutedResponse = await next.Invoke();

            // Return The Response to cashe it
            if (actionExecutedResponse.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
            {
                await _cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSecound));
            }

        }


        public string GenerateCacheKey(HttpRequest httpRequest)
        {
            // /api/product?PageIndex=1&pageSize=6&Sort=name

            var keyBulider = new StringBuilder();

            keyBulider.Append(httpRequest.Path); // /api/product


            foreach(var (key,value) in httpRequest.Query.OrderBy(x=>x.Key)) // Fix Issue When Send Different Order Qeuery Save As new Cashe response with same Data 
            {
                keyBulider.Append($"|{key}-{value}"); // |PageIndex-1|pageSize-6|Sort-name
            }

            return keyBulider.ToString();// /api/product|PageIndex-1|pageSize-6|Sort-name
        }
    }
}
