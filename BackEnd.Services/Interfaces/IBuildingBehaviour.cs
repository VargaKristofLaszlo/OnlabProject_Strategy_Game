using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBuildingBehaviour
    {
        public  City Upgrade(City city, BuildingUpgradeCost upgradeCost);
        public  City Downgrade(City city, BuildingUpgradeCost upgradeCost);
    }
}
