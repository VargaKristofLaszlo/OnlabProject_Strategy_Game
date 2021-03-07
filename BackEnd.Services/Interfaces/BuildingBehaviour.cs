using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public abstract class BuildingBehaviour
    {
        public abstract City Upgrade(City city, BuildingUpgradeCost upgradeCost);
        public abstract City Downgrade(City city, BuildingUpgradeCost upgradeCost);
    }
}
