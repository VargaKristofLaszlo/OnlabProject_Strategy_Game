using AutoMapper;
using BackEnd.Repositories.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetBuildingUpgradeCostsByName 
    { 
         public record Query(string BuildingName) : IRequest<List<Game.Shared.Models.Request.UpgradeCostCreationRequest>>;

        public class Handler : IRequestHandler<Query, List<Game.Shared.Models.Request.UpgradeCostCreationRequest>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;

            }

            public async Task<List<Game.Shared.Models.Request.UpgradeCostCreationRequest>> 
                Handle(Query request, CancellationToken cancellationToken)
            {
                var upgradeCosts = await _unitOfWork.UpgradeCosts.FindBuildingUpgradeCostsByName(request.BuildingName);

                var value = upgradeCosts
                    .Select(x => _mapper.Map<Game.Shared.Models.Request.UpgradeCostCreationRequest>(x))
                    .ToList();

                return value;
            }
        }
    }
}
