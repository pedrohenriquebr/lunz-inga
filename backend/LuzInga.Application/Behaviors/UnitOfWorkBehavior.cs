using LuzInga.Application.Abstractions.Messaging;
using LuzInga.Domain;
using MediatR;

namespace LuzInga.Application.Behaviors
{
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand
    {
        private readonly IUnitOfWork _dbContext;

        public UnitOfWorkBehavior(IUnitOfWork dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;
            try
            {
                _dbContext.BeginTransaction();
                response = await next();
                await _dbContext.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _dbContext.RollbackAsync();
                throw;
            }
            
            return response;
        }

    }

}