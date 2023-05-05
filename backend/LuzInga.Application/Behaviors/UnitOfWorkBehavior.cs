using FluentValidation;
using LuzInga.Application.Abstractions.Messaging;
using LuzInga.Domain;
using LuzInga.Domain.SharedKernel.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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
            IDbContextTransaction transaction = null;
            try
            {

                transaction  = _dbContext.Database.BeginTransaction();

                response = await next();

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                _dbContext.Dispose();
            }

            return response;
        }

    }

}