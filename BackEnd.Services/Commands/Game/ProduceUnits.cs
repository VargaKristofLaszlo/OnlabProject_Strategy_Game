using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using MediatR;
using Services.Exceptions;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands.Game
{
    public static class ProduceUnits
    {
        public record Command(UnitProductionRequest Request) : IRequest<Unit>;

        public class Handler : CityHandler, IRequestHandler<Command, Unit>
        {            
            private readonly IIdentityContext _identityContext;

            public Handler(IUnitOfWork unitOfWork, IIdentityContext identityContext): base(unitOfWork)
            {              
                _identityContext = identityContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var city = await GetCityByCityIndex(request.Request.CityIndex, _identityContext.UserId);

                var producibleUnits = await _unitOfWork.Units.GetProducibleUnitTypes(city.Barrack.Stage);

                (bool Producible, BackEnd.Models.Models.Unit Type) unitCheck 
                    = CheckIfUnitIsProducible(producibleUnits, request.Request.NameOfUnitType);

                if (unitCheck.Producible == false)
                    throw new NotFoundException();

                var productionCost = new BackEnd.Models.Models.Resources
                {
                    Population = request.Request.Amount * unitCheck.Type.UnitCost.Population,
                    Silver = request.Request.Amount * unitCheck.Type.UnitCost.Silver,
                    Stone = request.Request.Amount * unitCheck.Type.UnitCost.Stone,
                    Wood = request.Request.Amount * unitCheck.Type.UnitCost.Wood
                };

                if (CheckIfCityHasEnoughResources(city, productionCost) == false)
                    throw new BadRequestException("The city does not have enough resources");

                PayTheCost(city, productionCost);

                var unitsOfThisTypeInCity = await _unitOfWork.Units.GetUnitsInCity(unitCheck.Type.Id, city.Barrack.Id);

                //Create a new entry in the db
                if (unitsOfThisTypeInCity == null)
                {
                    await _unitOfWork.Units.InsertNewEntryToUnitsInCity(new BackEnd.Models.Models.UnitsInCity
                    {
                        Barrack = city.Barrack,
                        BarrackId = city.Barrack.Id,
                        Unit = unitCheck.Type,
                        UnitId = unitCheck.Type.Id,
                        Amount = request.Request.Amount
                    });
                }
                //Increase the amount in the city
                else
                    unitsOfThisTypeInCity.Amount += request.Request.Amount;

                await _unitOfWork.CommitChangesAsync();

                return new Unit();
            }

            private (bool Producible, BackEnd.Models.Models.Unit Type) 
                CheckIfUnitIsProducible(IEnumerable<BackEnd.Models.Models.Unit> units, string nameOfUnitType)
            {
                foreach (var item in units)
                {                    
                    if (item.Name.Equals(nameOfUnitType))
                        return (true, item);
                }
                return (false, null);
            }
        }
    }
}
