using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using Game.Shared.Models.Request;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(
                IUnitOfWork unitOfWork,
                IIdentityContext identityContext,
                IReportSender reportSender,
                UserManager<ApplicationUser> userManager)
            {
                _unitOfWork = unitOfWork;
                _identityContext = identityContext;
                _reportSender = reportSender;
                _userManager = userManager;
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

                //Update the attacking side
                foreach (var item in archeryPhaseResult.attackerTroops)
                {
                    foreach (var unitsInCity in unitsOfAttacker)
                    {
                        if (unitsInCity.Unit.Name.Equals(item.Key.Name))
                        {
                            var fallenSoldierAmount = (request.Request.AttackingForces.First(x => x.Key.Equals(item.Key.Name)).Value - item.Value);
                            unitsInCity.Amount -= fallenSoldierAmount;
                            initValues.attackerCity.Resources.Population += fallenSoldierAmount * item.Key.UnitCost.Population;
                        }
                    }
                }

                int initialWoodAmount = initValues.attackerCity.Resources.Wood;
                int initialStoneAmount = initValues.attackerCity.Resources.Stone;
                int initialSilverAmount = initValues.attackerCity.Resources.Silver;

                //Steal resources
                int totalCarryingCapacity = CalculateCarryingCapacity(archeryPhaseResult.attackerTroops);
                if (totalCarryingCapacity > 0)
                {
                    ResourceStealingProcess(initValues.attackerCity, initValues.defenderCity, totalCarryingCapacity);
                    CheckWarehouseCapacity(initValues.attackerCity);
                    if (request.Request.AttackType == AttackType.Conquer)
                    {
                        var conquerResult = await TryToConquerTheCity(
                            initValues.defenderCity,
                            initValues.attackerCity,
                            initValues.attackerName,
                            archeryPhaseResult.attackerTroops,
                            unitsOfAttacker);

                        unitsOfAttacker = conquerResult.unitsOfAttacker;
                        initValues.attackerCity = conquerResult.attackingCity;
                    }
                }

                int stolenWoodAmount = initValues.attackerCity.Resources.Wood - initialWoodAmount;
                int stolenStoneAmount = initValues.attackerCity.Resources.Stone - initialStoneAmount;
                int stolenSilverAmount = initValues.attackerCity.Resources.Silver - initialSilverAmount;

                await _reportSender.CreateReport(initValues.attackerName, initValues.attackerCity.CityName,
                    initValues.defenderName, initValues.defenderCity.CityName,
                    archeryPhaseResult.attackerTroops, archeryPhaseResult.defendingTroops,
                    request.Request.AttackingForces, initValues.defendingUnits, stolenWoodAmount, stolenStoneAmount, stolenSilverAmount);


                //Update the defending side
                foreach (var item in archeryPhaseResult.defendingTroops)
                {
                    var fallenSoldierAmount = defendingUnits.First(d => d.Unit.Name.Equals(item.Key.Name)).Amount - item.Value;
                    initValues.defenderCity.Resources.Population += fallenSoldierAmount * item.Key.UnitCost.Population;
                    defendingUnits.First(d => d.Unit.Name.Equals(item.Key.Name)).Amount = item.Value;
                }

                await _unitOfWork.CommitChangesAsync();

                return new MediatR.Unit();
            }

            private async Task<(IEnumerable<UnitsInCity> unitsOfAttacker, City attackingCity)> TryToConquerTheCity(
                City defenderCity,
                City attackingCity,
                string attackerName,
                Dictionary<BackEnd.Models.Models.Unit, int> archeryPhaseResult,
                IEnumerable<UnitsInCity> unitsOfAttacker)
            {
                var noble = await _unitOfWork.Units.FindUnitByName("Noble");

                int nobleCount;

                if (archeryPhaseResult.TryGetValue(noble, out nobleCount))
                {
                    Random loyaltyReduction = new Random();
                    defenderCity.Loyalty -= loyaltyReduction.Next(20, 30);

                    if (defenderCity.Loyalty <= 0)
                    {
                        var attackingUser = await _userManager.FindByNameAsync(attackerName);
                        defenderCity.User = attackingUser;
                        defenderCity.UserId = _identityContext.UserId;
                        defenderCity.Loyalty = 50;
                        attackingUser.Cities.Add(defenderCity);

                        foreach (var unitsInCity in unitsOfAttacker)
                        {
                            if (unitsInCity.Unit.Name.Equals("Noble"))
                            {
                                unitsInCity.Amount -= 1;
                            }
                        }

                        attackingCity.Resources.Population += noble.UnitCost.Population;
                    }
                }
                return (unitsOfAttacker, attackingCity);

            }

            private int ResourceStealingProcess(City attackerCity, City defenderCity, int totalCarryingCapacity)
            {
                int distributedCarryingCapacity = (int)Math.Floor((double)totalCarryingCapacity / 3);
                int leftover = 0;
                if (totalCarryingCapacity < 1)
                    return totalCarryingCapacity;

                if (defenderCity.Resources.Wood >= distributedCarryingCapacity)
                {
                    defenderCity.Resources.Wood -= distributedCarryingCapacity;
                    attackerCity.Resources.Wood += distributedCarryingCapacity;
                }
                else
                {
                    leftover += distributedCarryingCapacity - defenderCity.Resources.Wood;
                    attackerCity.Resources.Wood += defenderCity.Resources.Wood;
                    defenderCity.Resources.Wood = 0;
                }

                if (defenderCity.Resources.Stone >= distributedCarryingCapacity)
                {
                    defenderCity.Resources.Stone -= distributedCarryingCapacity;
                    attackerCity.Resources.Stone += distributedCarryingCapacity;
                }
                else
                {
                    leftover += distributedCarryingCapacity - defenderCity.Resources.Stone;
                    attackerCity.Resources.Stone += defenderCity.Resources.Stone;
                    defenderCity.Resources.Stone = 0;
                }

                if (defenderCity.Resources.Silver >= distributedCarryingCapacity)
                {
                    defenderCity.Resources.Silver -= distributedCarryingCapacity;
                    attackerCity.Resources.Silver += distributedCarryingCapacity;
                }
                else
                {
                    leftover += distributedCarryingCapacity - defenderCity.Resources.Silver;
                    attackerCity.Resources.Silver += defenderCity.Resources.Silver;
                    defenderCity.Resources.Silver = 0;
                }

                if (defenderCity.Resources.Wood == 0 && defenderCity.Resources.Silver == 0 && defenderCity.Resources.Stone == 0)
                    return 0;

                return ResourceStealingProcess(attackerCity, defenderCity, leftover);
            }

            private void CheckWarehouseCapacity(City city)
            {
                if (city.Resources.Wood > city.Warehouse.MaxWoodStorageCapacity)
                    city.Resources.Wood = city.Warehouse.MaxWoodStorageCapacity;

                if (city.Resources.Stone > city.Warehouse.MaxStoneStorageCapacity)
                    city.Resources.Stone = city.Warehouse.MaxStoneStorageCapacity;

                if (city.Resources.Silver > city.Warehouse.MaxSilverStorageCapacity)
                    city.Resources.Silver = city.Warehouse.MaxSilverStorageCapacity;
            }


            private int CalculateCarryingCapacity(Dictionary<BackEnd.Models.Models.Unit, int> attackerTroops)
            {
                int totalCarryingCapacity = 0;
                foreach (var item in attackerTroops)
                {
                    totalCarryingCapacity += item.Key.CarryingCapacity * item.Value;
                }

                return totalCarryingCapacity;
            }

            private async Task<(AttackingTroops attackingTroops, DefendingTroops defendingTroops,
           IEnumerable<UnitsInCity> unitsOfAttacker, IEnumerable<UnitsInCity> defendingUnits, int wallStage,
                string attackerName, City attackerCity, string defenderName, City defenderCity)> InitAttackProcess(AttackRequest request)
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
                    foreach (var type in allUnitTypes)
                    {
                        bool found = false;
                        foreach (var troop in request.AttackingForces)
                        {
                            if (troop.Key.Equals(type.Name))
                            {
                                attackingForces.Add(type, troop.Value);
                                found = true;
                            }
                        }
                        if (!found)
                            attackingForces.Add(type, 0);
                    }

                }
                catch (InvalidOperationException) { throw new BadRequestException("Invalid unit type name"); }

                AttackingTroops attackingTroops = new AttackingTroops(attackingForces);


                //Get the defending units
                var defendingUnits =
                    await _unitOfWork.Units.GetUnitsInCityByBarrackId(defendingUser.Cities.ElementAt(request.AttackedCityIndex).BarrackId);

                var tmp = defendingUnits.ToList();

                foreach (var type in allUnitTypes)
                {
                    bool found = false;
                    foreach (var troop in tmp)
                    {
                        if (troop.Unit.Name.Equals(type.Name))
                        {
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        tmp.Add(new UnitsInCity
                        {
                            Amount = 0,
                            Barrack = defendingCity.Barrack,
                            BarrackId = defendingCity.BarrackId,
                            Unit = type,
                            UnitId = type.Id
                        });
                    }
                }

                defendingUnits = tmp;


                DefendingTroops defendingTroops = new DefendingTroops(defendingUnits, attackingTroops.InfantryProvisionPercentage,
                    attackingTroops.CavalryProvisionPercentage, attackingTroops.ArcheryProvisionPercentage);

                int wallStage = defendingUser.Cities.ElementAt(request.AttackedCityIndex).CityWall.Stage;

                return (attackingTroops, defendingTroops, unitsOfAttacker, defendingUnits, wallStage,
                     attackingUser.UserName, attackingCity, defendingUser.UserName, defendingCity);
            }
        }
    }
}
