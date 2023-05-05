using MediatR;
namespace LuzInga.Application.Abstractions.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>
{

}
