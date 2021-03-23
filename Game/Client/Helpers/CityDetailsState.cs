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

        private readonly CityResourceState _cityResourceState;
        private readonly Game.Shared.IServices.IViewService _viewService;

        public CityDetailsState(CityResourceState cityResourceState, Game.Shared.IServices.IViewService viewService)
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
                    _cityResourceState.SetResourceValueAfterUpgrade(CityDetails.BarrackUpgradeCost);
                    CityDetails.BarrackUpgradeCost = newUpgradeCost;
                    CityDetails.BarrackStage = newStage;
                    break;
                case _cityHall:
                    _cityResourceState.SetResourceValueAfterUpgrade(CityDetails.CityHallUpgradeCost);
                    CityDetails.CityHallUpgradeCost = newUpgradeCost;
                    CityDetails.CityHallStage = newStage;
                    break;
                case _cityWall:
                    _cityResourceState.SetResourceValueAfterUpgrade(CityDetails.CityWallUpgradeCost);
                    CityDetails.CityWallUpgradeCost = newUpgradeCost;
                    CityDetails.CityWallStage = newStage;
                    break;
                case _farm:
                    _cityResourceState.SetResourceValueAfterUpgrade(CityDetails.FarmUpgradeCost);
                    CityDetails.FarmUpgradeCost = newUpgradeCost;
                    CityDetails.FarmStage = newStage;
                    break;
                case _lumber:
                    _cityResourceState.SetResourceValueAfterUpgrade(CityDetails.LumberUpgradeCost);
                    CityDetails.LumberUpgradeCost = newUpgradeCost;
                    CityDetails.LumberStage = newStage;
                    break;
                case _silverMine:
                    _cityResourceState.SetResourceValueAfterUpgrade(CityDetails.SilverMineUpgradeCost);
                    CityDetails.SilverMineUpgradeCost = newUpgradeCost;
                    CityDetails.SilverMineStage = newStage;
                    break;
                case _stoneMine:
                    _cityResourceState.SetResourceValueAfterUpgrade(CityDetails.StoneMineUpgradeCost);
                    CityDetails.StoneMineUpgradeCost = newUpgradeCost;
                    CityDetails.StoneMineStage = newStage;
                    break;
                case _wareHouse:
                    _cityResourceState.SetResourceValueAfterUpgrade(CityDetails.WarehouseUpgradeCost);
                    CityDetails.WarehouseUpgradeCost = newUpgradeCost;
                    CityDetails.WarehouseStage = newStage;
                    var capacity = await _viewService.GetWarehouseCapacity(cityIndex);                   
                    _cityResourceState.SetWarehouseCapacityAfterUpgrade(capacity);
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

                default:
                    break;
            }
            NotifyStateChanged();
        }


        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
