using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetWarehouseCapacity
    {
        public record Query(int CityIndex) : IRequest<WarehouseCapacity>;

        public class Handler : IRequestHandler<Query, WarehouseCapacity>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IIdentityContext _identityContext;

            public Handler(IUnitOfWork unitOfWork, IIdentityContext identityContext)
            {
                _unitOfWork = unitOfWork;
                _identityContext = identityContext;
            }

            public async Task<WarehouseCapacity> Handle(Query request, CancellationToken cancellationToken)
            {
                var warehouse = await _unitOfWork.Cities.FindWarehouseOfCity(request.CityIndex, _identityContext.UserId);

                return new WarehouseCapacity
                {
                    StoneLimit = warehouse.MaxStoneStorageCapacity,
                    SilverLimit = warehouse.MaxSilverStorageCapacity,
                    WoodLimit = warehouse.MaxWoodStorageCapacity
                };
            }
        }
    }
}