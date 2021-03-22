using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using MediatR;
using Services.Exceptions;
using Services.Implementations.AttackService.AttackPhaseBehaviourImpl;
using Services.Implementations.AttackService.Troops;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands
{
    public static class AttackOtherCity
    {
        public record Command(AttackRequest Request) : IRequest<MediatR.Unit>;

        public class Handler : IRequestHandler<Command, MediatR.Unit>
        {
            private readonly IIdentityContext _identityContext;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IReportSender _reportSender;

            public Handler(IUnitOfWork unitOfWork, IIdentityContext identityContext, IReportSender reportSender)
            {
                _unitOfWork = unitOfWork;
                _identityContext = identityContext;
                _reportSender = reportSender;
            }

            public async Task<MediatR.Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //Initialize the variables for the attack process
                var initValues = await InitAttackProcess(request.Request);
                AttackingTroops attackingTroops = initValues.attackingTroops;
                DefendingTroops defendingTroops = initValues.defendingTroops;
                IEnumerable<UnitsInCity> unitsOfAttacker = initValues.unitsOfAttacker;
                IEnumerable<UnitsInCity> defendingUnits = initValues.defendingUnits;
                int wallStage = initValues.wallStage;

                var infantryPhaseResult = new InfantryAttackPhaseBehaviour().Action(attackingTroops, defendingTroops, wallStage);

                //Add the survivors of the previous phase to the next one
                defendingTroops.AddSurvivorsOfPreviousPhase(infantryPhaseResult.defendingTroops, defendingTroops.CavalryPhaseDefendingUnits);
                attackingTroops.AddSurvivorsOfPreviousPhase(infantryPhaseResult.attackerTroops, attackingTroops.CavalryPhaseTroops);

                var cavalryPhaseResult = new CavalryAttackPhaseBehaviour().Action(attackingTroops, defendingTroops, wallStage);
                //Add the survivors of the previous phase to the next one
                defendingTroops.AddSurvivorsOfPreviousPhase(cavalryPhaseResult.defendingTroops, defendingTroops.ArcheryPhaseDefendingUnits);
                attackingTroops.AddSurvivorsOfPreviousPhase(cavalryPhaseResult.attackerTroops, attackingTroops.ArcheryPhaseTroops);

                var archeryPhaseResult = new ArcheryAttackPhaseBehaviour().Action(attackingTroops, defendingTroops, wallStage);

                await _reportSender.CreateReport(initValues.attackerName, initValues.attackerCityName,
                   initValues.defenderName, initValues.defenderCityName,
                   archeryPhaseResult.attackerTroops, archeryPhaseResult.defendingTroops,
                   request.Request.AttackingForces, initValues.defendingUnits);

                //Update the attacking and defending side
                foreach (var item in archeryPhaseResult.attackerTroops)
                    unitsOfAttacker.First(u => u.Unit.Name.Equals(item.Key.Name)).Amount 
                        -= request.Request.AttackingForces.First(x => x.Key.Equals(item.Key.Name)).Value - item.Value;
                         

                foreach (var item in archeryPhaseResult.defendingTroops)
                    defendingUnits.First(d => d.Unit.Name.Equals(item.Key.Name)).Amount = item.Value;

                await _unitOfWork.CommitChangesAsync();

                return new MediatR.Unit();
            }

            private async Task<(AttackingTroops attackingTroops, DefendingTroops defendingTroops,
           IEnumerable<UnitsInCity> unitsOfAttacker, IEnumerable<UnitsInCity> defendingUnits, int wallStage,
                string attackerName, string attackerCityName, string defenderName, string defenderCityName)> InitAttackProcess(AttackRequest request)
            {
                var attackingUser = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);
                var defendingUser = await _unitOfWork.Users.GetUserWithCities(request.AttackedUserId);
                if (defendingUser == null || attackingUser == null)
                    throw new NotFoundException();

                var attackingCity = attackingUser.Cities.ElementAt(request.AttackerCityIndex);
                if (attackingCity == null)
                    throw new NotFoundException();

                var defendingCity = defendingUser.Cities.ElementAt(request.AttackedCityIndex);
                if (defendingCity == null)
                    throw new NotFoundException();


                var unitsOfAttacker = await _unitOfWork.Units.GetUnitsInCityByBarrackId(attackingCity.BarrackId);
                IEnumerable<BackEnd.Models.Models.Unit> allUnitTypes = await _unitOfWork.Units.GetAllUnitsAsync();

                //Convert the dto into a model in order to use it for the attack calculations
                Dictionary<BackEnd.Models.Models.Unit, int> attackingForces = new Dictionary<BackEnd.Models.Models.Unit, int>();
                try
                {
                    foreach (var item in request.AttackingForces)
                        attackingForces.Add(allUnitTypes.First(unit => unit.Name.Equals(item.Key)), item.Value);
                }
                catch (InvalidOperationException) { throw new BadRequestException("Invalid unit type name"); }

                AttackingTroops attackingTroops = new AttackingTroops(attackingForces);

                //Get the defending units
                var defendingUnits =
                    await _unitOfWork.Units.GetUnitsInCityByBarrackId(defendingUser.Cities.ElementAt(request.AttackedCityIndex).BarrackId);

                DefendingTroops defendingTroops = new DefendingTroops(defendingUnits, attackingTroops.InfantryProvisionPercentage,
                    attackingTroops.CavalryProvisionPercentage, attackingTroops.ArcheryProvisionPercentage);

                int wallStage = defendingUser.Cities.ElementAt(request.AttackedCityIndex).CityWall.Stage;

                return (attackingTroops, defendingTroops, unitsOfAttacker, defendingUnits, wallStage,
                     attackingUser.UserName, attackingCity.CityName, defendingUser.UserName, defendingCity.CityName);
            }
        }
    }
}
