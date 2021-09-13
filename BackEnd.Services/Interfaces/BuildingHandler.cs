using Services.Exceptions;
using Services.Implementations.BuildingBehaviourImpl;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public abstract class BuildingHandler
    {
        protected IBuildingBehaviour CreateConcreteBuildingBehaviour(string buildingName)
        {
            return buildingName switch
            {
                "Barrack" => new BarrackBehaviour(),
                "CityHall" => new CityHallBehaviour(),
                "CityWall" => new CityWallBehaviour(),
                "Farm" => new FarmBehaviour(),
                "SilverMine" => new SilverMineBehaviour(),
                "StoneMine" => new StoneMineBehaviour(),
                "Lumber" => new LumberBehaviour(),
                "Warehouse" => new WarehouseBehaviour(),
                "Castle" => new CastleBehaviour(),
                _ => throw new NotFoundException(),
            };
        }

        protected static void ValidateBuildingName(string buildingName)
        {
            if (new List<string>()
            {
                "Barrack","CityHall", "CityWall", "Farm", "SilverMine", "StoneMine", "Lumber", "Warehouse", "Castle"
            }.Contains(buildingName) == false)
                throw new NotFoundException();
        }
    }
}
