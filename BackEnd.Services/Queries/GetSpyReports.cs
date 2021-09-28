using BackEnd.Infrastructure;
using Game.Shared.Models.Response;
using MediatR;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetSpyReports
    {
        public record Query(int PageNumber, int PageSize) : IRequest<CollectionResponse<Game.Shared.Models.SpyReport>>;

        public class Handler : IRequestHandler<Query, CollectionResponse<Game.Shared.Models.SpyReport>>
        {
            private readonly IIdentityContext _identityContext;
            private readonly IReportSender _reportSender;

            public Handler(IIdentityContext identityContext, IReportSender reportSender)
            {
                _identityContext = identityContext;
                _reportSender = reportSender;
            }

            public async Task<CollectionResponse<Game.Shared.Models.SpyReport>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _reportSender.GetSpyReports(request.PageNumber, request.PageSize, _identityContext.Username);
            }
        }
    }
}
