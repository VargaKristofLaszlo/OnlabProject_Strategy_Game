using Services.Exceptions;
using Services.Implementations.BuildingBehaviourImpl;

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
                _ => throw new NotFoundException(),
            };
        }
    }
}
