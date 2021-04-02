using MediatR;
using System.Threading.Tasks;
using Game.Shared.Models;
using BackEnd.Repositories.Interfaces;
using AutoMapper;
using System.Threading;
using System.Linq;

namespace Services.Queries
{
    public class GetBuildingQueueByUserID
    {
        public record Query(string UserId) : IRequest<BuildingQueue>;

        public class Handler : IRequestHandler<Query, BuildingQueue>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<BuildingQueue> Handle(Query request, CancellationToken cancellationToken)
            {
                var jobs = await _unitOfWork.HangFire.GetUserBuildingQueue(request.UserId);
                
                var queue = jobs.Select(x => _mapper.Map<QueueData>(x)).ToList();

                return new BuildingQueue() { Queue = queue };
            }
        }
    }
}
