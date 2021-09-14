namespace BackEnd.Models.Models
{
    public class Tavern : BuildingRecord
    {
        public int SpyCount { get; set; }
        public int MaximumSpyCount { get; set; }

        public static Tavern Create(BuildingUpgradeCost upgradeCost)
        {
            return new Tavern
            {
                Stage = 0,
                BuildingName = upgradeCost.BuildingName,
                BuildingCostId = upgradeCost.Id,
                UpgradeCost = upgradeCost,
                SpyCount = 0,
                MaximumSpyCount = 1
            };
        }
    }
}
