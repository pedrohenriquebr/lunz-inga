using MediatR;
namespace LuzInga.Application.Abstractions.Messaging;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
        where TCommand : ICommand
{
}