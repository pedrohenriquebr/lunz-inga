using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuzInga.Application.Events;
using LuzInga.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace LuzInga.Application.Services
{
    public interface IAuditLogger
    { 
        public Task LogRecent(ApplicationAccessedEvent data);
    }
}