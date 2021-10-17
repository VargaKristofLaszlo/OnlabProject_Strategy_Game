using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using MediatR;
using Services.Exceptions;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands.Game
{
    public static class RecruitNewSpy
    {
        public record Command(int Amount, int CityIndex) : IRequest<MediatR.Unit>;

        public class Handler : CityHandler, IRequestHandler<Command, MediatR.Unit>
        {
            private readonly IIdentityContext _identityContext;

            public Handler(IUnitOfWork unitOfWork, IIdentityContext identityContext)
                : base(unitOfWork)
            {
                _identityContext = identityContext;
            }

            public async Task<MediatR.Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var city = await GetCityByCityIndex(request.CityIndex, _identityContext.UserId);

                var spyRecruitment = _unitOfWork.UpgradeCosts.FindBuildingUpgradeCostsByName("Spy").Result.First().UpgradeCost;

                var paidResources = new Resources()
                {
                    Population = 0,
                    Silver = spyRecruitment.Silver * request.Amount,
                    Stone = spyRecruitment.Stone * request.Amount,
                    Wood = spyRecruitment.Wood * request.Amount
                };

                if (CheckIfCityHasEnoughResources(city, paidResources) == false)
                    throw new BadRequestException("The city does not have enough resources");

                if (city.Tavern.MaximumSpyCount < request.Amount + city.Tavern.SpyCount)
                    throw new BadRequestException("You cant recruit more spies, please upgrade your tavern");

                PayTheCost(city, paidResources);

                city.Tavern.SpyCount += request.Amount;

                await _unitOfWork.CommitChangesAsync();

                return new MediatR.Unit();
            }
        }
    }
}
