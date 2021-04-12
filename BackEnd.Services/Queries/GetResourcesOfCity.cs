using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using MediatR;
using Services.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetResourcesOfCity
    {
        public record Query(int CityIndex) : IRequest<CityResources>;

        public class Handler : IRequestHandler<Query, CityResources>
        {
            private readonly IUnitOfWork _unitOfWork;          
            private readonly IIdentityContext _identityContext;

            public Handler(IUnitOfWork unitOfWork, IIdentityContext identityContext)
            {
                _unitOfWork = unitOfWork;
                _identityContext = identityContext;
            }

            public async Task<CityResources> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);

                if (user == null)
                    throw new NotFoundException();

                var city = await _unitOfWork.Cities.FindCityById(user.Cities[request.CityIndex].Id);
                
                return new CityResources
                {
                    StoneAmount = city.Resources.Stone,
                    StoneProductionPerHour = city.StoneProduction.ProductionAmount,
                    SilverAmount = city.Resources.Silver,
                    SilverProductionPerHour = city.SilverProduction.ProductionAmount,
                    WoodAmount = city.Resources.Wood,
                    WoodProductionPerHour = city.WoodProduction.ProductionAmount,
                    TotalPopulation = city.Farm.MaxPopulation,
                    FreePopulation = city.Resources.Population
                };
            }
        }
    }
}
