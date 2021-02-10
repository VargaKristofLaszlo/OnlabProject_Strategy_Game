using BackEnd.Models.Models;
using Shared.Models.Request;

namespace BackEnd.Services.Extensions
{
    public static class AdminServiceExtensions
    {
        public static BuildingUpgradeCost ModifyValues(this BuildingUpgradeCost previousCost, UpgradeCostCreationRequest newCost)
        {
            previousCost.UpgradeCost.Wood = newCost.Wood;
            previousCost.UpgradeCost.Silver = newCost.Silver;
            previousCost.UpgradeCost.Stone = newCost.Stone;
            previousCost.UpgradeCost.Population = newCost.Population;

            return previousCost;
        }

    }
}
