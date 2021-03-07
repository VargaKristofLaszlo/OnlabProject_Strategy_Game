using BackEnd.Models.Models;
using Game.Shared.Models.Request;
using System;

namespace BackEnd.Services.Extensions
{
    public static class AdminServiceExtensions
    {
        public static BuildingUpgradeCost ModifyValues(this BuildingUpgradeCost previousCost, UpgradeCostCreationRequest newCost)
        {
            previousCost.UpgradeCost.Wood = newCost.UpgradeCost.Wood;
            previousCost.UpgradeCost.Silver = newCost.UpgradeCost.Silver;
            previousCost.UpgradeCost.Stone = newCost.UpgradeCost.Stone;
            previousCost.UpgradeCost.Population = newCost.UpgradeCost.Population;
            previousCost.UpgradeTime = TimeSpan.FromSeconds(newCost.UpgradeTimeInSeconds);
            return previousCost;
        }

    }
}
