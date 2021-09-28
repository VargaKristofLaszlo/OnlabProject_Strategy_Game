using AutoMapper;
using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetTavernDetails
    {
        public record Query(int CityIndex) : IRequest<TavernDetails> { }

        public class Handler : IRequestHandler<Query, TavernDetails>
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

            public async Task<TavernDetails> Handle(Query request, CancellationToken cancellationToken)
            {
                var userWithCities = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);

                var tavern = userWithCities.Cities[request.CityIndex].Tavern;

                var spyCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Spy", 1);

                return new TavernDetails()
                {
                    MaximumSpyCount = tavern.MaximumSpyCount,
                    AvailableSpyCount = tavern.SpyCount,
                    SpyCost = _mapper.Map<Resources>(spyCost.UpgradeCost),
                    Stage = tavern.Stage
                };
            }
        }
    }
}
