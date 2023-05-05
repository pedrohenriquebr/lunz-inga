using MediatR;

namespace LuzInga.Application.Abstractions.Messaging;

public interface IQueryObject<out TResponse>  : IRequest<TResponse>
{

}
