using MediatR;
using System.Threading.Tasks;
using Game.Shared.Models;
using BackEnd.Repositories.Interfaces;
using AutoMapper;
using System.Threading;
using System.Linq;

namespace Services.Queries
{
    public class GetUnitRecruitQueueByUserId
    {
        public record Query(string UserId) : IRequest<UnitQueue>;

        public class Handler : IRequestHandler<Query, UnitQueue>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<UnitQueue> Handle(Query request, CancellationToken cancellationToken)
            {
                var jobs = await _unitOfWork.HangFire.GetUserUnitProductionQueue(request.UserId);

                var queue = jobs.Select(x => _mapper.Map<UnitQueueData>(x)).ToList();

                return new UnitQueue() { Queue = queue };
            }
        }
    }
}
