using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuzInga.Application.Configuration
{
    public class RedisConfig
    {
        public string ApplicationPrefixKey {get; set;}
        public string KeyDelimiter {get; set;}
        public string AuditListKey {get; set;}
    }
}