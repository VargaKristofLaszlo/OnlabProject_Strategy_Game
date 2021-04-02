using MediatR;
using System;
namespace Hangfire.MediatR
{
    public static class MediatorExtensions
    {
        public static string Enqueue(this IMediator mediator, string jobName, IRequest<Unit> request)
        {
            var client = new BackgroundJobClient();
            return client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request));
        }


        public static string Schedule(this IMediator mediator, string jobName, IRequest<Unit> request, DateTime startTime)
        {
            var client = new BackgroundJobClient();
            return client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request), startTime);
        }
    }
}