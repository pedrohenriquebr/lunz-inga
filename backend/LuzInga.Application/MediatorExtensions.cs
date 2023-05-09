using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using LuzInga.Application.Abstractions.Messaging;
using LuzInga.Domain.SharedKernel;
using MediatR;

namespace LuzInga.Application
{
    public static class MediatorExtensions
    {

        public static void Enqueue<T>(this IMediator mediator, T data)
            where T : ICommand
        {
            BackgroundJob.Enqueue<IMediator>(x => x.Send<T>(data, default));
        }

        public static void EnqueueEvent<T>(this IMediator mediator, T data)
            where T : BaseEvent
        {
            BackgroundJob.Enqueue<IMediator>(x => x.Publish<T>(data, default));
        }
    }
}