using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace LuzInga.Application.Extensions;

public static class DistributedCacheExtensions
{

    private static int DEFAULT_EXPIRETIME = 60;

    public static async Task SetRecordAsync<T>(this IDistributedCache cache,
        string recordId,
        T data,
        TimeSpan? absExpireTime = null,
        TimeSpan? uExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = absExpireTime ?? TimeSpan.FromSeconds(DEFAULT_EXPIRETIME),
            SlidingExpiration = uExpireTime
        };

        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, options);
    }


    public static async Task SetRequestAsync(this IDistributedCache cache,
        string recordId,
        string data,
        TimeSpan? absExpireTime = null,
        TimeSpan? uExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = absExpireTime ?? TimeSpan.FromSeconds(DEFAULT_EXPIRETIME),
            SlidingExpiration = uExpireTime
        };

        await cache.SetStringAsync(recordId, data, options);
    }


    public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
    {
        var jsonData = await cache.GetStringAsync(recordId);

        if (jsonData is null)
            return default(T);

        return JsonSerializer.Deserialize<T>(jsonData);
    }


    public static async Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string recordId, Func<Task<T>> handler)
    {
        var fromCache = await cache.GetRecordAsync<T>(recordId);

        if (fromCache is null)
        {
            var result = await handler.Invoke();
            await cache.SetRecordAsync(recordId, result);
            return result;
        }

        return fromCache;
    }
}
