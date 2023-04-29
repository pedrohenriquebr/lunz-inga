using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LuzInga.Application.Events;
using LuzInga.Application.Services;
using LuzInga.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;

namespace LuzInga.Infra.Services.Redis
{
    public class AuditLogger : IAuditLogger
    {

        private readonly IConnectionMultiplexer redis;
        private readonly RedisKey fullKey;
        private JsonSerializerOptions serializerOptions;
        private IDatabase db;

        public AuditLogger(IConnectionMultiplexer redis, RedisKey fullkey)
        {
            this.redis = redis;
            this.fullKey  = fullkey;
            serializerOptions = new JsonSerializerOptions(){
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public async Task LogRecent(ApplicationAccessedEvent request)
        {
            this.db = redis.GetDatabase();
            await db.ListLeftPushAsync(this.fullKey, 
                            new StringBuilder()
                            .Append(request.Datetime)
                            .Append("     ")
                            .Append("Recent")
                            .Append("%")
                            .Append(request.Url)
                            .Append("%")
                            .Append(request.Username)
                            .Append("%")
                            .Append(request.Method)
                            .Append("%")
                            .Append(request.RemoteIpAddress)
                            .ToString());
            await db.ListTrimAsync(this.fullKey, 0, 99);
        }
    }
}