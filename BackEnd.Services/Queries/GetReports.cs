using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Response;
using MediatR;
using Models.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetReports
    {
        public record Query(int PageNumber, int PageSize) : IRequest<CollectionResponse<Game.Shared.Models.Report>>;

        public class Handler : IRequestHandler<Query, CollectionResponse<Game.Shared.Models.Report>>
        {            
            private readonly IIdentityContext _identityContext;
            private readonly IReportSender _reportSender;

            public Handler(IIdentityContext identityContext, IReportSender reportSender)
            {               
                _identityContext = identityContext;
                _reportSender = reportSender;
            }

            public async Task<CollectionResponse<Game.Shared.Models.Report>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _reportSender.GetReports(request.PageNumber, request.PageSize, _identityContext.Username);
            }
        }
    }
}
