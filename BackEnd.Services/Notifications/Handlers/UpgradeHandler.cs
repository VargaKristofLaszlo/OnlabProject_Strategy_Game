
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Notifications.Handlers
{
    public class UpgradeHandler : INotificationHandler<UpgradeNotification>
    {
        private readonly IMediator _mediator;

        public UpgradeHandler(IMediator mediator)
        {            
            _mediator = mediator;
        }

        public async Task Handle(UpgradeNotification notification, CancellationToken cancellationToken)
        {
            
        }
    }
}
