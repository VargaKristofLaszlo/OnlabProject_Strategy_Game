using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using Services.Exceptions;
using Services.Implementations.AttackService.AttackPhaseBehaviourImpl;
using Services.Implementations.AttackService.Troops;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Implementations.AttackService
{
    public class AttackService : IAttackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityContext _identityContext;      

        public AttackService(IUnitOfWork unitOfWork, IIdentityContext identityOptions)
        {
            _unitOfWork = unitOfWork;
            _identityContext = identityOptions;            
        }

        /*
            The fight is divided into 3 phases:
                -infantry
                -cavalry
                -archers
            Each phase the attacker uses the units with the type according to the phase.
                So in the infantry phase the attacker can only use infantry units.
            The defender can use all of it's unit types in every phase however the amount used is limited.
            The used amount is calculated via this formula:
               percentage = (sum of population cost of the unit type used in the current phase) / (sum of population cost of all the attacking units)
               For every defending unit type: amount =  (amount of units of this type) * percentage
            After this calculate the attack values: 
                Sum of (attack points of the unit type) * (amount of units)
            The defense values: 
                Sum of (defense points of the unit type according to the phase) * (amount of units)
            The side with the lower sum looses all units.
            The other side suffers casualties using this formula:
                ratio = (Sqrt of (defenseValue / attackValue)) / (attackValue / defenseValue)
                Remaining amount of unit type = current amount * ratio
            The remaining troops are added to the next phase as extras.

         */
        public async Task AttackOtherCity(AttackRequest request)
        {
            //Initialize the variables for the attack process
            var initValues = await InitAttackProcess(request);
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

            //Update the attacking and defending side
            foreach (var item in archeryPhaseResult.attackerTroops)
                unitsOfAttacker.Where(u => u.Unit.Name.Equals(item.Key.Name)).First().Amount = item.Value;

            foreach (var item in archeryPhaseResult.defendingTroops)
                defendingUnits.Where(d => d.Unit.Name.Equals(item.Key.Name)).First().Amount = item.Value;

            await _unitOfWork.CommitChangesAsync();
        }


        private async Task<(AttackingTroops attackingTroops, DefendingTroops defendingTroops,
            IEnumerable<UnitsInCity> unitsOfAttacker, IEnumerable<UnitsInCity> defendingUnits, int wallStage)> InitAttackProcess(AttackRequest request)
        {
            var attackingUser = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);
            var defendingUser = await _unitOfWork.Users.GetUserWithCities(request.AttackedUserId);
            if (defendingUser == null || attackingUser == null)
                throw new NotFoundException();

            var attackingCity = attackingUser.Cities.ElementAt(request.AttackerCityIndex);
            if (attackingCity == null)
                throw new NotFoundException();

            var unitsOfAttacker = await _unitOfWork.Units.GetUnitsInCityByBarrackId(attackingCity.BarrackId);
            IEnumerable<Unit> allUnitTypes = await _unitOfWork.Units.GetAllUnitsAsync();

            //Convert the dto into a model in order to use it for the attack calculations
            Dictionary<Unit, int> attackingForces = new Dictionary<Unit, int>();
            try
            {
                foreach (var item in request.AttackingForces)
                    attackingForces.Add(allUnitTypes.First(unit => unit.Name.Equals(item)), item.Value);
            }
            catch (InvalidOperationException) { throw new BadRequestException("Invalid unit type name"); }

            AttackingTroops attackingTroops = new AttackingTroops(attackingForces);

            //Get the defending units
            var defendingUnits =
                await _unitOfWork.Units.GetUnitsInCityByBarrackId(defendingUser.Cities.ElementAt(request.AttackedCityIndex).BarrackId);

            DefendingTroops defendingTroops = new DefendingTroops(defendingUnits, attackingTroops.InfantryProvisionPercentage,
                attackingTroops.CavalryProvisionPercentage, attackingTroops.ArcheryProvisionPercentage);

            int wallStage = defendingUser.Cities.ElementAt(request.AttackedCityIndex).CityWall.Stage;

            return (attackingTroops, defendingTroops, unitsOfAttacker, defendingUnits, wallStage);
        }
    }
}
