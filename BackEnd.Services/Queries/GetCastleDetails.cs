using AutoMapper;
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
    public static class GetCastleDetails
    {
        public record Query(int CityIndex) : IRequest<CastleDetails> { }

        public class Handler : IRequestHandler<Query, CastleDetails>
        {
            private readonly IIdentityContext _identityContext;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(
                IIdentityContext identityContext,
                IUnitOfWork unitOfWork, IMapper mapper)
            {
                _identityContext = identityContext;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<CastleDetails> Handle(Query request, CancellationToken cancellationToken)
            {
                var userWithCities = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);

                var castle = userWithCities.Cities[request.CityIndex].Castle;

                var coinCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Coin", 1);
                var unitTypeNoble = await _unitOfWork.Units.FindUnitByName("Noble");

                return new CastleDetails()
                {
                    AvailableCoinCount = castle.AvailableCoinCount,
                    MaximumCoinCount = castle.MaximumCoinCount - castle.UsedCoinCount,
                    Stage = castle.Stage,
                    CoinCost = _mapper.Map<Resources>(coinCost.UpgradeCost),
                    NobleCost = _mapper.Map<Resources>(unitTypeNoble.UnitCost)
                };
            }
        }
    }
}
