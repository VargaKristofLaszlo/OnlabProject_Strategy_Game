using BackEnd.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DataSeeding
{
    public class UnitTypeSeeding
    {
        private readonly ApplicationDbContext _db;
        private Unit Spearman;
        private Unit Swordsman;
        private Unit AxeFighter;
        private Unit Archer;
        private Unit LightCavalry;
        private Unit MountedArcher;
        private Unit HeavyCavalry;
        public UnitTypeSeeding(ApplicationDbContext db)
        {
            _db = db;

            //Creating the unit types
            Spearman = new Unit
            {
                Name = "Spearman",
                AttackPoint = 10,
                DefensePoint = 25,
                MinBarrackStage = 1,
                CarryingCapacity = 25,
                UnitCost = new Resources
                {
                    Wood = 50,
                    Stone = 30,
                    Silver = 20,
                    Population = 1
                }
            };
            Swordsman = new Unit
            {
                Name = "Swordsman",
                AttackPoint = 25,
                DefensePoint = 55,
                MinBarrackStage = 3,
                CarryingCapacity = 15,
                UnitCost = new Resources
                {
                    Wood = 30,
                    Stone = 30,
                    Silver = 70,
                    Population = 1
                }
            };
            AxeFighter = new Unit
            {
                Name = "Axe Fighter",
                AttackPoint = 45,
                DefensePoint = 10,
                MinBarrackStage = 5,
                CarryingCapacity = 20,
                UnitCost = new Resources
                {
                    Wood = 60,
                    Stone = 30,
                    Silver = 40,
                    Population = 1
                }
            };
            Archer = new Unit
            {
                Name = "Archer",
                AttackPoint = 25,
                DefensePoint = 50,
                MinBarrackStage = 9,
                CarryingCapacity = 10,
                UnitCost = new Resources
                {
                    Wood = 80,
                    Stone = 30,
                    Silver = 60,
                    Population = 1
                }
            };
            LightCavalry = new Unit
            {
                Name = "Light Cavalry",
                AttackPoint = 130,
                DefensePoint = 40,
                MinBarrackStage = 11,
                CarryingCapacity = 50,
                UnitCost = new Resources
                {
                    Wood = 125,
                    Stone = 100,
                    Silver = 250,
                    Population = 4
                }
            };
            MountedArcher = new Unit
            {
                Name = "Mounted Archer",
                AttackPoint = 150,
                DefensePoint = 50,
                MinBarrackStage = 13,
                CarryingCapacity = 50,
                UnitCost = new Resources
                {
                    Wood = 250,
                    Stone = 200,
                    Silver = 100,
                    Population = 5
                }
            };
            HeavyCavalry = new Unit
            {
                Name = "Heavy Cavalry",
                AttackPoint = 150,
                DefensePoint = 200,
                MinBarrackStage = 21,
                CarryingCapacity = 50,
                UnitCost = new Resources
                {
                    Wood = 200,
                    Stone = 150,
                    Silver = 600,
                    Population = 6
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

        private async Task CreateUnitType(Unit unitType)
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
