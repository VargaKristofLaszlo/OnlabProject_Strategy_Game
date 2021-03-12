using BackEnd.Repositories.Interfaces;
using BackEnd.Services.Extensions;
using Game.Shared.Models.Request;
using MediatR;
using Services.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands.Admin
{
    public static class ModifyBuildingUpgradeCost
    {
        public record Command(UpgradeCostCreationRequest Request) : IRequest<Unit>;

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;              
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.UpgradeCosts.FindUpgradeCost(request.Request.BuildingName, request.Request.BuildingStage);

                if (result == null)
                    throw new NotFoundException();

                result.ModifyValues(request.Request);

                await _unitOfWork.CommitChangesAsync();

                return new Unit();
            }
        }
    }
}
