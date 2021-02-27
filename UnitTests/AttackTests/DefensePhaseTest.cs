using BackEnd.Models.Models;
using FluentAssertions;
using Game.Shared.Models;
using Services.Implementations.AttackService.DefensePhaseBehaviourImpl;
using Services.Implementations.AttackService.Troops;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.AttackTests
{
    public class DefensePhaseTest
    {
        public static Dictionary<BackEnd.Models.Models.Unit, int> emptyDefendingTroops = new Dictionary<BackEnd.Models.Models.Unit, int>();
        public static IEnumerable<object[]> emptyTroops = new List<object[]> { new object[] { emptyDefendingTroops, 0 } };

        public static Dictionary<BackEnd.Models.Models.Unit, int> infantryDefendingTroops = new Dictionary<BackEnd.Models.Models.Unit, int>()
        {
            {
            new BackEnd.Models.Models.Unit
                {
                    Name = "Spearman",
                    AttackPoint = 10,
                    InfantryDefensePoint = 25,
                    CavalryDefensePoint = 45,
                    ArcherDefensePoint = 10,
                    UnitType = UnitType.Infantry,
                    MinBarrackStage = 1,
                    CarryingCapacity = 25,
                    UnitCost = new BackEnd.Models.Models.Resources
                    {
                        Wood = 50,
                        Stone = 30,
                        Silver = 20,
                        Population = 1
                    }
                },10
            },
            {
            new BackEnd.Models.Models.Unit
                 {
                    Name = "Swordsman",
                    AttackPoint = 25,
                    InfantryDefensePoint = 55,
                    CavalryDefensePoint = 5,
                    ArcherDefensePoint = 30,
                    UnitType = UnitType.Infantry,
                    MinBarrackStage = 3,
                    CarryingCapacity = 15,
                    UnitCost = new BackEnd.Models.Models.Resources
                    {
                        Wood = 30,
                        Stone = 30,
                        Silver = 70,
                        Population = 1
                    }
                },10
            }
        };
        public static IEnumerable<object[]> infantryTroops = new List<object[]> { new object[] { infantryDefendingTroops, 800 } };

        public static Dictionary<BackEnd.Models.Models.Unit, int> allKindOfDefendingTroops = new Dictionary<BackEnd.Models.Models.Unit, int>()
        {
            {
            new BackEnd.Models.Models.Unit
                {
                    Name = "Spearman",
                    AttackPoint = 10,
                    InfantryDefensePoint = 25,
                    CavalryDefensePoint = 45,
                    ArcherDefensePoint = 10,
                    UnitType = UnitType.Infantry,
                    MinBarrackStage = 1,
                    CarryingCapacity = 25,
                    UnitCost = new BackEnd.Models.Models.Resources
                    {
                        Wood = 50,
                        Stone = 30,
                        Silver = 20,
                        Population = 1
                    }
                },10
            },
            {
            new BackEnd.Models.Models.Unit
                {
                    Name = "Archer",
                    AttackPoint = 25,
                    InfantryDefensePoint = 10,
                    CavalryDefensePoint = 30,
                    ArcherDefensePoint = 60,
                    UnitType = UnitType.Archer,
                    MinBarrackStage = 9,
                    CarryingCapacity = 10,
                    UnitCost = new BackEnd.Models.Models.Resources
                    {
                        Wood = 80,
                        Stone = 30,
                        Silver = 60,
                        Population = 1
                    }
                },10
            },
            {
                new BackEnd.Models.Models.Unit
                {
                        Name = "Heavy Cavalry",
                        AttackPoint = 150,
                        InfantryDefensePoint = 200,
                        CavalryDefensePoint = 160,
                        ArcherDefensePoint = 180,
                        UnitType = UnitType.Cavalry,
                        MinBarrackStage = 21,
                        CarryingCapacity = 50,
                        UnitCost = new BackEnd.Models.Models.Resources
                        {
                            Wood = 200,
                            Stone = 150,
                            Silver = 600,
                            Population = 6
                        }
                },10
            }
        };
        public static IEnumerable<object[]> allKindOfTroops = new List<object[]> { new object[] { allKindOfDefendingTroops, 2500 } };

        public static Dictionary<BackEnd.Models.Models.Unit, int> attackingAxeFighters = new Dictionary<BackEnd.Models.Models.Unit, int>()
        {
            {
                new BackEnd.Models.Models.Unit
                {
                    Name = "Axe Fighter",
                    AttackPoint = 45,
                    InfantryDefensePoint= 10,
                    CavalryDefensePoint = 5,
                    ArcherDefensePoint = 10,
                    UnitType = UnitType.Infantry,
                    MinBarrackStage = 5,
                    CarryingCapacity = 20,
                    UnitCost = new BackEnd.Models.Models.Resources
                    {
                        Wood = 60,
                        Stone = 30,
                        Silver = 40,
                        Population = 1
                    }
                },20
            }
        };
        public static IEnumerable<object[]> axeFighers = new List<object[]> { new object[] { attackingAxeFighters, 900 } };

        public static Dictionary<BackEnd.Models.Models.Unit, int> allKindOfAttackingTroops = new Dictionary<BackEnd.Models.Models.Unit, int>()
        {
            {
            new BackEnd.Models.Models.Unit
                {
                    Name = "Spearman",
                    AttackPoint = 10,
                    InfantryDefensePoint = 25,
                    CavalryDefensePoint = 45,
                    ArcherDefensePoint = 10,
                    UnitType = UnitType.Infantry,
                    MinBarrackStage = 1,
                    CarryingCapacity = 25,
                    UnitCost = new BackEnd.Models.Models.Resources
                    {
                        Wood = 50,
                        Stone = 30,
                        Silver = 20,
                        Population = 1
                    }
                },10
            },
            {
            new BackEnd.Models.Models.Unit
                {
                    Name = "Archer",
                    AttackPoint = 25,
                    InfantryDefensePoint = 10,
                    CavalryDefensePoint = 30,
                    ArcherDefensePoint = 60,
                    UnitType = UnitType.Archer,
                    MinBarrackStage = 9,
                    CarryingCapacity = 10,
                    UnitCost = new BackEnd.Models.Models.Resources
                    {
                        Wood = 80,
                        Stone = 30,
                        Silver = 60,
                        Population = 1
                    }
                },10
            },
            {
                new BackEnd.Models.Models.Unit
                {
                        Name = "Heavy Cavalry",
                        AttackPoint = 150,
                        InfantryDefensePoint = 200,
                        CavalryDefensePoint = 160,
                        ArcherDefensePoint = 180,
                        UnitType = UnitType.Cavalry,
                        MinBarrackStage = 21,
                        CarryingCapacity = 50,
                        UnitCost = new BackEnd.Models.Models.Resources
                        {
                            Wood = 200,
                            Stone = 150,
                            Silver = 600,
                            Population = 6
                        }
                },10
            }
        };
        public static IEnumerable<object[]> attackingTroops = new List<object[]> { new object[] { allKindOfAttackingTroops, 1850 } };

        public static IEnumerable<object[]> dataForAttackersWonTest = new List<object[]> { new object[]
            { attackingAxeFighters, infantryDefendingTroops, 200, 50 } };
        
        public static IEnumerable<object[]> dataForDefendersWonTest = new List<object[]> { new object[]
            { attackingAxeFighters, infantryDefendingTroops, 50, 200 } };

        [Theory]
        [MemberData(nameof(emptyTroops))]
        [MemberData(nameof(infantryTroops))]
        public void InfantryDefenseCalculationTest(Dictionary<BackEnd.Models.Models.Unit, int> defendingTroops, int expectedDefenseValue)
        {            
            InfantryDefensePhase infantryDefense = new InfantryDefensePhase();
            int defenseValue = infantryDefense.CalculateDefenseValue(defendingTroops);

            defenseValue.Should().Be(expectedDefenseValue);
        }
        
        [Theory]
        [MemberData(nameof(emptyTroops))]        
        [MemberData(nameof(allKindOfTroops))]
        public void ArcherDefenseCalculationTest(Dictionary<BackEnd.Models.Models.Unit, int> defendingTroops, int expectedDefenseValue)
        {
            //Attack and defense values for the infantry phase
            ArcheryDefensePhase archeryDefense = new ArcheryDefensePhase();
            int defenseValue = archeryDefense.CalculateDefenseValue(defendingTroops);

            defenseValue.Should().Be(expectedDefenseValue);
        }

        [Theory]
        [MemberData(nameof(emptyTroops))]
        [MemberData(nameof(axeFighers))]
        [MemberData(nameof(attackingTroops))]
        public void AttackValueCalculationTest(Dictionary<BackEnd.Models.Models.Unit, int> attackingforces, int expectedAttackValue)
        {
            AttackingTroops attackingTroops = new AttackingTroops(attackingforces);

            int attackValue = attackingTroops.CalculateAttackValue(attackingforces);

            attackValue.Should().Be(expectedAttackValue);
        }

        [Theory]
        [MemberData(nameof(dataForAttackersWonTest))]
        public void AttackersWonTheFightTest
            (Dictionary<BackEnd.Models.Models.Unit, int> attackingforces, Dictionary<BackEnd.Models.Models.Unit, int> defendingForces,
                int attackValue, int defenseValue)
        {
            AttackingTroops attackingTroops = new AttackingTroops(attackingforces);

            var attackingForcesBeforeTheFight = new Dictionary<BackEnd.Models.Models.Unit, int>(attackingforces);

            var fightResult = attackingTroops.Fight(attackingforces, defendingForces, attackValue, defenseValue);

            foreach (var item in fightResult.attackingTroops)            
                item.Value.Should().BeLessThan(attackingForcesBeforeTheFight[item.Key]);

            foreach (var item in fightResult.defendingTroops)            
                item.Value.Should().Be(0);
        }


        [Theory]
        [MemberData(nameof(dataForDefendersWonTest))]
        public void DefendersWonTest             
            (Dictionary<BackEnd.Models.Models.Unit, int> attackingforces, Dictionary<BackEnd.Models.Models.Unit, int> defendingForces,
                int attackValue, int defenseValue)
        {
            AttackingTroops attackingTroops = new AttackingTroops(attackingforces);

            var defendingForcesBeforeTheFight = new Dictionary<BackEnd.Models.Models.Unit, int>(defendingForces);

            var fightResult = attackingTroops.Fight(attackingforces, defendingForces, attackValue, defenseValue);

            foreach (var item in fightResult.defendingTroops)
                item.Value.Should().BeLessThan(defendingForcesBeforeTheFight[item.Key]);

            foreach (var item in fightResult.attackingTroops)
                item.Value.Should().Be(0);
        }
    }
}
