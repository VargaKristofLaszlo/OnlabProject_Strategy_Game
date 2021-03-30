using AutoMapper;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using MediatR;
using Services.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands.Admin
{
    public static class CreateBuildingUpgradeCost
    {
        public record Command(UpgradeCostCreationRequest request) : IRequest<Unit>;

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var previousMaxStage = await _unitOfWork.UpgradeCosts.FindMaxStage(request.request.BuildingName);

                if (previousMaxStage.HasValue)
                {
                    // Only be able to create an upgrade cost for the next stage
                    if (previousMaxStage.Value + 1 != request.request.BuildingStage)
                        throw new BadRequestException("You can only creat an upgrade cost for the next stage");
                }
                else
                {
                    if (request.request.BuildingStage != 1)
                        throw new BadRequestException("If you want to create an upgrade cost for a new building" +
                            " create a cost for stage 1 first");
                }

                await _unitOfWork.UpgradeCosts.CreateAsync(_mapper.Map<BackEnd.Models.Models.BuildingUpgradeCost>(request.request));
                await _unitOfWork.CommitChangesAsync();

                return new Unit();
            }
        }
    }
}
