using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace LuzInga.Application.Extensions;

public static class DistributedCacheExtensions
{

    public static async Task SetRecordAsync<T>(this IDistributedCache cache,
        string recordId,
        T data,
        TimeSpan? absExpireTime = null,
        TimeSpan? uExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions(){
                AbsoluteExpirationRelativeToNow = absExpireTime ?? TimeSpan.FromSeconds(60),
                SlidingExpiration = uExpireTime
            };

            var jsonData  = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }


        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache,string recordId){
            var jsonData = await cache.GetStringAsync(recordId);

            if(jsonData is null)
                return default(T);
                
            return JsonSerializer.Deserialize<T>(jsonData);
        }
}
