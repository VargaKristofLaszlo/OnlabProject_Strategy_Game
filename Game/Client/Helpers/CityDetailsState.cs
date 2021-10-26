using Game.Shared.Models;
using Game.Shared.Services;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public class CityDetailsState
    {
        private const string _barrack = "Barrack";
        private const string _cityHall = "CityHall";
        private const string _cityWall = "CityWall";
        private const string _farm = "Farm";
        private const string _lumber = "Lumber";
        private const string _silverMine = "SilverMine";
        private const string _stoneMine = "StoneMine";
        private const string _wareHouse = "Warehouse";
        private const string _castle = "Castle";
        private const string _tavern = "Tavern";

        private readonly CityResourceState _cityResourceState;
        private readonly Game.Shared.IServices.IViewService _viewService;

        public CityDetailsState(
            CityResourceState cityResourceState,
            Game.Shared.IServices.IViewService viewService
            )
        {
            _cityResourceState = cityResourceState;
            _viewService = viewService;
        }

        public CityDetails CityDetails { get; private set; }
        public event Action OnChange;

        public void Init(CityDetails initValue)
        {
            CityDetails = initValue;
            NotifyStateChanged();
        }

        public async Task BuildingUpgrade(string buildingName, Resources newUpgradeCost, int newStage, int cityIndex)
        {
            switch (buildingName)
            {
                case _barrack:
                    CityDetails.BarrackUpgradeCost = newUpgradeCost;
                    CityDetails.BarrackStage = newStage;
                    break;
                case _cityHall:
                    CityDetails.CityHallUpgradeCost = newUpgradeCost;
                    CityDetails.CityHallStage = newStage;
                    break;
                case _cityWall:
                    CityDetails.CityWallUpgradeCost = newUpgradeCost;
                    CityDetails.CityWallStage = newStage;
                    break;
                case _farm:
                    CityDetails.FarmUpgradeCost = newUpgradeCost;
                    CityDetails.FarmStage = newStage;
                    break;
                case _lumber:
                    CityDetails.LumberUpgradeCost = newUpgradeCost;
                    CityDetails.LumberStage = newStage;
                    break;
                case _silverMine:
                    CityDetails.SilverMineUpgradeCost = newUpgradeCost;
                    CityDetails.SilverMineStage = newStage;
                    break;
                case _stoneMine:
                    CityDetails.StoneMineUpgradeCost = newUpgradeCost;
                    CityDetails.StoneMineStage = newStage;
                    break;
                case _wareHouse:
                    CityDetails.WarehouseUpgradeCost = newUpgradeCost;
                    CityDetails.WarehouseStage = newStage;
                    var capacity = await _viewService.GetWarehouseCapacity(cityIndex);
                    _cityResourceState.SetWarehouseCapacityAfterUpgrade(capacity);
                    break;
                case _castle:
                    CityDetails.CastleUpgradeCost = newUpgradeCost;
                    CityDetails.CastleStage = newStage;
                    break;
                case _tavern:
                    CityDetails.TavernUpgradeCost = newUpgradeCost;
                    CityDetails.TavernStage = newStage;
                    break;
                default:
                    break;
            }
            NotifyStateChanged();
        }

        public void BuildingDowngrade(string buildingName, Resources newUpgradeCost, int newStage)
        {
            switch (buildingName)
            {
                case _barrack:
                    CityDetails.BarrackUpgradeCost = newUpgradeCost;
                    CityDetails.BarrackStage = newStage;
                    break;
                case _cityHall:
                    CityDetails.CityHallUpgradeCost = newUpgradeCost;
                    CityDetails.CityHallStage = newStage;
                    break;
                case _cityWall:
                    CityDetails.CityWallUpgradeCost = newUpgradeCost;
                    CityDetails.CityWallStage = newStage;
                    break;
                case _farm:
                    CityDetails.FarmUpgradeCost = newUpgradeCost;
                    CityDetails.FarmStage = newStage;
                    break;
                case _lumber:
                    CityDetails.LumberUpgradeCost = newUpgradeCost;
                    CityDetails.LumberStage = newStage;
                    break;
                case _silverMine:
                    CityDetails.SilverMineUpgradeCost = newUpgradeCost;
                    CityDetails.SilverMineStage = newStage;
                    break;
                case _stoneMine:
                    CityDetails.StoneMineUpgradeCost = newUpgradeCost;
                    CityDetails.StoneMineStage = newStage;
                    break;
                case _wareHouse:
                    CityDetails.WarehouseUpgradeCost = newUpgradeCost;
                    CityDetails.WarehouseStage = newStage;
                    break;
                case _castle:
                    CityDetails.CastleUpgradeCost = newUpgradeCost;
                    CityDetails.CastleStage = newStage;
                    break;
                case _tavern:
                    CityDetails.TavernUpgradeCost = newUpgradeCost;
                    CityDetails.TavernStage = newStage;
                    break;
                default:
                    break;
            }
            NotifyStateChanged();
        }


        public int GetBuildingStage(string buildingName)
        {
            return buildingName switch
            {
                _barrack => CityDetails.BarrackStage,
                _cityHall => CityDetails.CityHallStage,
                _cityWall => CityDetails.CityWallStage,
                _farm => CityDetails.FarmStage,
                _lumber => CityDetails.LumberStage,
                _silverMine => CityDetails.SilverMineStage,
                _stoneMine => CityDetails.StoneMineStage,
                _wareHouse => CityDetails.WarehouseStage,
                _castle => CityDetails.CastleStage,
                _tavern => CityDetails.TavernStage,
                _ => -1,
            };
        }

        public Resources GetBuildingUpgradeCost(string buildingName)
        {
            return buildingName switch
            {
                _barrack => CityDetails.BarrackUpgradeCost,
                _cityHall => CityDetails.CityHallUpgradeCost,
                _cityWall => CityDetails.CityWallUpgradeCost,
                _farm => CityDetails.FarmUpgradeCost,
                _lumber => CityDetails.LumberUpgradeCost,
                _silverMine => CityDetails.SilverMineUpgradeCost,
                _stoneMine => CityDetails.StoneMineUpgradeCost,
                _wareHouse => CityDetails.WarehouseUpgradeCost,
                _castle => CityDetails.CastleUpgradeCost,
                _tavern => CityDetails.TavernUpgradeCost,
                _ => null,
            };
        }

        public BuildingInformation CreateBuildingInformation(string displayedBuildingName)
        {
            return displayedBuildingName switch
            {
                "City hall" => new BuildingInformation()
                {
                    Header = $"{displayedBuildingName} (Level {CityDetails.CityHallStage})",
                    Description = "The city hall is the heart of your city. Upgrade it and your other buildings' upgrade time will decrease",
                    Content = null,
                    UpgradeCost = CityDetails.CityHallUpgradeCost,
                    ImgSource = "/images/buildings/cityHall.svg",
                },
                "City wall" => new BuildingInformation()
                {
                    Header = $"{displayedBuildingName} (Level {CityDetails.CityWallStage})",
                    Description = "This building protects your village from attackers. Upgrade it to increase the defending bonuses it gives",
                    Content = new { DefenseValue = CityDetails.CityWallDefenseValue, Multiplier = CityDetails.CityWallMultiplier },
                    UpgradeCost = CityDetails.CityWallUpgradeCost,
                    ImgSource = "/images/buildings/Citywall.svg",
                },
                "Farm" => new BuildingInformation()
                {
                    Header = $"{displayedBuildingName} (Level {CityDetails.CityWallStage})",
                    Description = "This building determines how much population you have." +
                        "Population is used for upgrading buildings and producing units." +
                        "Upgrade it to increase the maximum population of the city",
                    Content = new { Population = CityDetails.MaximumPopulation },
                    UpgradeCost = CityDetails.FarmUpgradeCost,
                    ImgSource = "/images/buildings/farm.svg",
                },
                "Lumber" => new BuildingInformation()
                {
                    Header = $"{displayedBuildingName} (Level {CityDetails.LumberStage})",
                    Description =
                "This building produces the resource Wood in regular intervals. Upgrade this building to increase the production amount",
                    Content = new { Production = CityDetails.WoodProduction },
                    UpgradeCost = CityDetails.LumberUpgradeCost,
                    ImgSource = "/images/buildings/lumber.svg",
                },
                "Silver mine" => new BuildingInformation()
                {
                    Header = $"{displayedBuildingName} (Level {CityDetails.SilverMineStage})",
                    Description =
                "This building produces the resource Silver in regular intervals. Upgrade this building to increase the production amount",
                    Content = new { Production = CityDetails.SilverProduction },
                    UpgradeCost = CityDetails.SilverMineUpgradeCost,
                    ImgSource = "/images/buildings/SilverMine.svg"
                },
                "Stone mine" => new BuildingInformation()
                {
                    Header = $"{displayedBuildingName} (Level {CityDetails.StoneMineStage})",
                    Description =
                "This building produces the resource Stone in regular intervals. Upgrade this building to increase the production amount",
                    Content = new { Production = CityDetails.StoneProduction },
                    UpgradeCost = CityDetails.StoneMineUpgradeCost,
                    ImgSource = "/images/buildings/StoneMine.svg"
                },
                "Warehouse" => new BuildingInformation()
                {
                    Header = $"{displayedBuildingName} (Level {CityDetails.WarehouseStage})",
                    Description =
                "This building stores the resources of the city. Upgrade this building to increase the stored amount",
                    Content = new { Storage = CityDetails.MaximumStorage },
                    UpgradeCost = CityDetails.WarehouseUpgradeCost,
                    ImgSource = "/images/buildings/warehouse.svg"
                },
                _ => null
            };
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
