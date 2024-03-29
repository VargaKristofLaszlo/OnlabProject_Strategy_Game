﻿using System.ComponentModel;
using System.Threading.Tasks;
using MediatR;

namespace Hangfire.MediatR
{
    public class MediatorHangfireBridge
    {
        private readonly IMediator _mediator;

        public MediatorHangfireBridge(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Send(IRequest<Unit> command)
        {
            await _mediator.Send(command);
        }

        [DisplayName("{0}")]
        public async Task Send(string jobName, IRequest<Unit> command)
        {
            await _mediator.Send(command);
        }
    }
}