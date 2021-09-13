using BackEnd.Models.Models;
using Game.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DataSeeding
{
    public class UnitTypeSeeding
    {
        private readonly ApplicationDbContext _db;
        private BackEnd.Models.Models.Unit Spearman;
        private BackEnd.Models.Models.Unit Swordsman;
        private BackEnd.Models.Models.Unit AxeFighter;
        private BackEnd.Models.Models.Unit Archer;
        private BackEnd.Models.Models.Unit LightCavalry;
        private BackEnd.Models.Models.Unit MountedArcher;
        private BackEnd.Models.Models.Unit HeavyCavalry;
        private BackEnd.Models.Models.Unit Noble;

        public UnitTypeSeeding(ApplicationDbContext db)
        {
            _db = db;

            //Creating the unit types
            Spearman = new BackEnd.Models.Models.Unit
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
            };
            Swordsman = new BackEnd.Models.Models.Unit
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
            };
            AxeFighter = new BackEnd.Models.Models.Unit
            {
                Name = "Axe Fighter",
                AttackPoint = 45,
                InfantryDefensePoint = 10,
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
            };
            Archer = new BackEnd.Models.Models.Unit
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
            };
            LightCavalry = new BackEnd.Models.Models.Unit
            {
                Name = "Light Cavalry",
                AttackPoint = 130,
                InfantryDefensePoint = 30,
                CavalryDefensePoint = 40,
                ArcherDefensePoint = 30,
                UnitType = UnitType.Cavalry,
                MinBarrackStage = 11,
                CarryingCapacity = 50,
                UnitCost = new BackEnd.Models.Models.Resources
                {
                    Wood = 125,
                    Stone = 100,
                    Silver = 250,
                    Population = 4
                }
            };
            MountedArcher = new BackEnd.Models.Models.Unit
            {
                Name = "Mounted Archer",
                AttackPoint = 150,
                InfantryDefensePoint = 40,
                CavalryDefensePoint = 30,
                ArcherDefensePoint = 50,
                UnitType = UnitType.Archer,
                MinBarrackStage = 13,
                CarryingCapacity = 50,
                UnitCost = new BackEnd.Models.Models.Resources
                {
                    Wood = 250,
                    Stone = 200,
                    Silver = 100,
                    Population = 5
                }
            };
            HeavyCavalry = new BackEnd.Models.Models.Unit
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
            };
            Noble = new BackEnd.Models.Models.Unit
            {
                Name = "Noble",
                AttackPoint = 1,
                InfantryDefensePoint = 1,
                CavalryDefensePoint = 1,
                ArcherDefensePoint = 1,
                UnitType = UnitType.Archer,
                MinBarrackStage = 0,
                CarryingCapacity = 50,
                UnitCost = new BackEnd.Models.Models.Resources
                {
                    Wood = 100,
                    Stone = 100,
                    Silver = 100,
                    Population = 5
                }
            };
        }

        public async Task SeedUnitTypes()
        {
            await CreateUnitType(Spearman);
            await CreateUnitType(Swordsman);
            await CreateUnitType(AxeFighter);
            await CreateUnitType(Archer);
            await CreateUnitType(LightCavalry);
            await CreateUnitType(MountedArcher);
            await CreateUnitType(HeavyCavalry);
        }

        private async Task CreateUnitType(BackEnd.Models.Models.Unit unitType)
        {
            var _unitType = await _db.Units
              .Where(unit => unit.Name.Equals(unitType.Name))
              .FirstOrDefaultAsync();

            if (_unitType != null)
                return;

            await _db.Units.AddAsync(unitType);

            await _db.SaveChangesAsync();
        }
    }
}
