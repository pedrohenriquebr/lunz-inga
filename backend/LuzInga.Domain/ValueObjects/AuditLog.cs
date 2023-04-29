using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuzInga.Domain.ValueObjects
{
    public sealed record AuditLog(
        DateTime DateTime,
        string? Url,
        string? UserName,
        string? UserId,
        string? HttpMethod,
        string? RemoteIp,
        bool isError,
        string? ErrorMessage
    );
}