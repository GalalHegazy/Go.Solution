using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Core.Services.Contract
{
    public interface ICacheService
    {
        //Set Responce in MemoryDb
        Task CacheResponseAsync(string key , object response , TimeSpan timeToLive);
        //Get Responce From MemoryDb
        Task<String?> GetCacheResponseAsync(string key);
    }
}
