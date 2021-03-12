using AutoMapper;
using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using MediatR;
using Services.Exceptions;
using Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands.Game
{
    public static class SendResourcesToOtherPlayer
    {
        public record Command(SendResourceToOtherPlayerRequest Request) : IRequest<Unit>;

        public class Handler : CityHandler, IRequestHandler<Command, Unit>
        {
            private readonly IIdentityContext _identityContext;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork, IIdentityContext identityContext, IMapper mapper) : base(unitOfWork)
            {
                _identityContext = identityContext;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var senderCity = await GetCityByCityIndex(request.Request.FromCityIndex, _identityContext.UserId);

                var receivingUser = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(request.Request.ToUserName);
                var receivingCity = await GetCityByCityIndex(request.Request.ToCityIndex, receivingUser.Id);

                var sentResources = _mapper.Map<BackEnd.Models.Models.Resources>(request.Request);

                if (CheckIfCityHasEnoughResources(senderCity, sentResources) == false)
                    throw new BadRequestException("The city does not have enough resources");

                var silverAmount = receivingCity.Resources.Silver + sentResources.Silver;
                var stoneAmount = receivingCity.Resources.Stone + sentResources.Stone;
                var woodAmount = receivingCity.Resources.Wood + sentResources.Wood;

                CheckWarehouseCapacity(ref stoneAmount, ref silverAmount, ref woodAmount, receivingCity.Warehouse);

                receivingCity.Resources.Wood = woodAmount;
                receivingCity.Resources.Silver = silverAmount;
                receivingCity.Resources.Stone = stoneAmount;

                PayTheCost(senderCity, sentResources);
                await _unitOfWork.CommitChangesAsync();

                return new Unit();
            }

            private void CheckWarehouseCapacity(ref int stoneAmount, ref int silverAmount, ref int woodAmount,
                BackEnd.Models.Models.Warehouse warehouse)
            {
                if (stoneAmount > warehouse.MaxStoneStorageCapacity)
                    stoneAmount = warehouse.MaxStoneStorageCapacity;

                if (silverAmount > warehouse.MaxSilverStorageCapacity)
                    silverAmount = warehouse.MaxSilverStorageCapacity;

                if (woodAmount > warehouse.MaxWoodStorageCapacity)
                    woodAmount = warehouse.MaxWoodStorageCapacity;
            }
        }
    }
}
