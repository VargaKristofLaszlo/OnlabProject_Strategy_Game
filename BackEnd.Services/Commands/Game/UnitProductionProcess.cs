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
    public static class UnitProductionProcess
    {
        public record Command(int CityIndex, string UnitName, int Amount, IIdentityContext IdentityContext,
            DateTime FinishTime) : IRequest<Unit>;

        public class Handler : CityHandler, IRequestHandler<Command, Unit>
        {
            public Handler(IUnitOfWork unitOfWork) : base(unitOfWork)
            {

            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var city = await GetCityByCityIndex(request.CityIndex, request.IdentityContext.UserId);

                var unit = await _unitOfWork.Units.FindUnitByName(request.UnitName);

                var unitsOfThisTypeInCity = await _unitOfWork.Units.GetUnitsInCity(unit.Id, city.Barrack.Id);

                if (request.UnitName == "Noble")
                {
                    if (city.Castle.AvailableCoinCount >= 1 == false)
                        throw new BadRequestException("Not enough coins");

                    city.Castle.AvailableCoinCount--;
                    city.Castle.UsedCoinCount++;
                }

                //Create a new entry in the db
                if (unitsOfThisTypeInCity == null)
                {
                    await _unitOfWork.Units.InsertNewEntryToUnitsInCity(new BackEnd.Models.Models.UnitsInCity
                    {
                        Barrack = city.Barrack,
                        BarrackId = city.Barrack.Id,
                        Unit = unit,
                        UnitId = unit.Id,
                        Amount = request.Amount
                    });
                }
                //Increase the amount in the city
                else
                    unitsOfThisTypeInCity.Amount += request.Amount;

                if (!request.UnitName.Equals("Noble"))
                {
                    var job = await _unitOfWork.HangFire
                   .GetUnitJobByFinishTime(request.FinishTime, request.CityIndex, request.IdentityContext.UserId);
                    _unitOfWork.HangFire.RemoveBuildingJob(job);
                }

                await _unitOfWork.CommitChangesAsync();

                return new Unit();
            }

        }
    }
}
