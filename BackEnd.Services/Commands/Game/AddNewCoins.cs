using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using MediatR;
using SendGrid.Helpers.Errors.Model;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands.Game
{
    public static class AddNewCoins
    {
        public record Command(CoinCreationRequest Request) : IRequest<MediatR.Unit>;

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
                var city = await GetCityByCityIndex(request.Request.CityIndex, _identityContext.UserId);

                var singleCoinCost = _unitOfWork.UpgradeCosts.FindBuildingUpgradeCostsByName("Coin").Result.First().UpgradeCost;

                var paidResources = new Resources()
                {
                    Population = 0,
                    Silver = singleCoinCost.Silver * request.Request.Amount,
                    Stone = singleCoinCost.Stone * request.Request.Amount,
                    Wood = singleCoinCost.Wood * request.Request.Amount
                };

                if (CheckIfCityHasEnoughResources(city, paidResources) == false)
                    throw new BadRequestException("The city does not have enough resources");

                if (city.Castle.MaximumCoinCount - city.Castle.UsedCoinCount - city.Castle.AvailableCoinCount < request.Request.Amount)
                    throw new BadRequestException("You cant create the requested amount of coins, please upgrade your castle");

                PayTheCost(city, paidResources);

                city.Castle.AvailableCoinCount += request.Request.Amount;

                await _unitOfWork.CommitChangesAsync();

                return new MediatR.Unit();
            }
        }
    }
}
