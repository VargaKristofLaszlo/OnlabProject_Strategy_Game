namespace BackEnd.Models.Models
{
    public class Castle : BuildingRecord
    {
        public int CoinCount { get; set; }

        public static Castle Create(BuildingUpgradeCost upgradeCost)
        {
            return new Castle
            {
                Stage = 0,
                BuildingName = upgradeCost.BuildingName,
                BuildingCostId = upgradeCost.Id,
                UpgradeCost = upgradeCost,
                CoinCount = 0
            };
        }
    }
}
