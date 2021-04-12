using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using MediatR;
using Services.Exceptions;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands.Game
{
    public static class UnitProductionStart
    {
        public record Command(UnitProductionRequest Request, IIdentityContext IdentityContext) : IRequest<DateTime>;

        public class Handler : CityHandler, IRequestHandler<Command, DateTime>
        {


            public Handler(IUnitOfWork unitOfWork) : base(unitOfWork) { }          

            public async Task<DateTime> Handle(Command request, CancellationToken cancellationToken)
            {
                var city = await GetCityByCityIndex(request.Request.CityIndex, request.IdentityContext.UserId);

                var producibleUnits = await _unitOfWork.Units.GetProducibleUnitTypes(city.Barrack.Stage);

                BackEnd.Models.Models.Unit  unit
                    = CheckIfUnitIsProducible(producibleUnits, request.Request.NameOfUnitType);

                if(unit == null)
                    throw new NotFoundException();

                var productionCost = new BackEnd.Models.Models.Resources
                {
                    Population = request.Request.Amount * unit.UnitCost.Population,
                    Silver = request.Request.Amount * unit.UnitCost.Silver,
                    Stone = request.Request.Amount * unit.UnitCost.Stone,
                    Wood = request.Request.Amount * unit.UnitCost.Wood
                };

                if (CheckIfCityHasEnoughResources(city, productionCost) == false)
                    throw new BadRequestException("The city does not have enough resources");

                PayTheCost(city, productionCost);

                var finishTime = await _unitOfWork.HangFire.GetUnitFinishTime(request.IdentityContext.UserId);

                var recruitTime = new TimeSpan(0,0,(int)Math.Ceiling(unit.RecruitTime.TotalSeconds) * request.Request.Amount);

                var newFinishTime = finishTime.Add(recruitTime);

                await _unitOfWork.HangFire.AddNewUnitProductionJob(request.IdentityContext.UserId, finishTime, newFinishTime,
                    request.Request.NameOfUnitType, request.Request.Amount, request.Request.CityIndex);

                await _unitOfWork.CommitChangesAsync();

                return newFinishTime;
            }

            private BackEnd.Models.Models.Unit
                CheckIfUnitIsProducible(IEnumerable<BackEnd.Models.Models.Unit> units, string nameOfUnitType)
            {
                foreach (var item in units)
                {
                    if (item.Name.Equals(nameOfUnitType))
                        return item;
                }
                return null;
            }
        }
    }
}
