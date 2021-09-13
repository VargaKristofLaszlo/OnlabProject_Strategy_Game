namespace BackEnd.Models.Models
{
    public class Castle : BuildingRecord
    {
        public int MaximumCoinCount { get; set; }
        public int AvailableCoinCount { get; set; }

        public static Castle Create(BuildingUpgradeCost upgradeCost)
        {
            return new Castle
            {
                Stage = 0,
                BuildingName = upgradeCost.BuildingName,
                BuildingCostId = upgradeCost.Id,
                UpgradeCost = upgradeCost,
                MaximumCoinCount = 0,
                AvailableCoinCount = 0
            };
        }
    }
}
