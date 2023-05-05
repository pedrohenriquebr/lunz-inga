using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LuzInga.Application.Services
{
    public interface IKeyFactory
    {
        public string CreateCachingKey(PathString path, IQueryCollection query);
        public string CreateGlobalInstancePrefix();
        public string CreateAuditPrefix();
    }
}