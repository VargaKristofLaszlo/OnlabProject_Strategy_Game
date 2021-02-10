using BackEnd.Models.Models;
using Shared.Models;
using Shared.Models.Request;
using System;

namespace BackEnd.Services.Extensions
{
    public static class MapperExtensions
    {
        public static Models.Models.BuildingUpgradeCost ToBuildingUpgradeCostModel(this UpgradeCostCreationRequest request)
        {
            return new Models.Models.BuildingUpgradeCost
            {
                BuildingName = request.BuildingName,
                BuildingStage = request.BuildingStage,
                UpgradeCost = new Resources
                {
                    Wood = request.Wood,
                    Stone = request.Stone,
                    Silver = request.Silver,
                    Population = request.Population
                },
                UpgradeTime = TimeSpan.Zero
            };
        }

        public static Shared.Models.BuildingUpgradeCost ToBuildingUpgradeCostDto(this Models.Models.BuildingUpgradeCost model) 
        {
            return new Shared.Models.BuildingUpgradeCost
            {
                Population = model.UpgradeCost.Population,
                Wood = model.UpgradeCost.Wood,
                Stone = model.UpgradeCost.Stone,
                Silver = model.UpgradeCost.Silver
            };
        }

        public static Credentials ToCredentialsDto(this ApplicationUser user)
        {
            return new Credentials
            {
                Email = user.Email,
                Username = user.UserName,
                Id = user.Id
            };
        }
    }
}
