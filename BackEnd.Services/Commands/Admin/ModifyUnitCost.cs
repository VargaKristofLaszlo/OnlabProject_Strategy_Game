using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using MediatR;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands.Admin
{
    public static class ModifyUnitCost
    {
        public record Command(UnitCostModificationRequest Request) : IRequest<Unit>;

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;            

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Units.FindUnitByName(request.Request.Name);

                if (result == null)
                    throw new NotFoundException();

                result.UnitCost.Wood = request.Request.Wood;
                result.UnitCost.Stone = request.Request.Stone;
                result.UnitCost.Silver = request.Request.Silver;
                result.UnitCost.Population = request.Request.Population;

                await _unitOfWork.CommitChangesAsync();

                return new Unit();
            }
        }
    }
}
