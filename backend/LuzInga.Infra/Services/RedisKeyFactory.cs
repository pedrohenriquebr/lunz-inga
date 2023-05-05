using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuzInga.Application.Configuration;
using LuzInga.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LuzInga.Infra.Services;

public class RedisKeyFactory : IRedisKeyFactory
{
    private const string CACHING_PREFIX = "Caching";
    private readonly IOptions<RedisConfig> config;

    public RedisKeyFactory(IOptions<RedisConfig> redisConfig)
    {
        this.config = redisConfig;
    }

    public string CreateAuditPrefix()
        => CreateInstanceNamePrefix(config)
                    .Append(config.Value.AuditListKey)
                          .ToString();

    public string CreateCachingKey(PathString path, IQueryCollection query)
    {
        var sb = new StringBuilder();
        sb.Append(CACHING_PREFIX);
        sb.Append(config.Value.KeyDelimiter);
        sb.Append(path);

        foreach (var item in query)
        {
            sb.Append(config.Value.KeyDelimiter);
            sb.Append(item.Key);
            sb.Append(config.Value.KeyDelimiter);
            sb.Append(item.Value);
        }

        return sb.ToString();
    }

    public string CreateGlobalInstancePrefix()
        => CreateInstanceNamePrefix(this.config)
            .ToString();

    private static StringBuilder CreateInstanceNamePrefix(IOptions<RedisConfig> redisConfig)
    {
        return new StringBuilder()
                    .Append(redisConfig.Value.ApplicationPrefixKey)
                    .Append(redisConfig.Value.KeyDelimiter);
    }
}