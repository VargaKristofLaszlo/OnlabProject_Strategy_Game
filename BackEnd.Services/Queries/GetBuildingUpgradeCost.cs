using AutoMapper;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using MediatR;
using SendGrid.Helpers.Errors.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetBuildingUpgradeCost
    {
        public record Query(string BuildingName, int BuildingStage) : IRequest<BuildingUpgradeCost>;

        public class Handler : IRequestHandler<Query, BuildingUpgradeCost>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<BuildingUpgradeCost> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.UpgradeCosts.FindUpgradeCost(request.BuildingName, request.BuildingStage);

                if (result == null)
                    throw new NotFoundException();

                return _mapper.Map<BuildingUpgradeCost>(result);
            }
        }
    }
}
