using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace LuzInga.Application.Abstractions.Messaging
{
    public interface IQueryObjectHandler<in TQueryObject, TResponse> : IRequestHandler<TQueryObject, TResponse>
        where TQueryObject : IQueryObject<TResponse>
    {
        
    }
}