using AutoMapper;
using BackEnd.Models.Models;
using Game.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityDetails>()
                .ForMember(detail => detail.BarrackStage, map => map.MapFrom(city => city.Barrack.Stage))
                .ForMember(detail => detail.BarrackUpgradeCost, map => map.MapFrom(city => city.Barrack.UpgradeCost.UpgradeCost))
                .ForMember(detail => detail.CityHallStage, map => map.MapFrom(city => city.CityHall.Stage))
                .ForMember(detail => detail.CityHallUpgradeCost, map => map.MapFrom(city => city.CityHall.UpgradeCost.UpgradeCost))
                .ForMember(detail => detail.CityName, map => map.MapFrom(city => city.CityName))
                .ForMember(detail => detail.CityWallStage, map => map.MapFrom(city => city.CityWall.Stage))
                .ForMember(detail => detail.CityWallUpgradeCost, map => map.MapFrom(city => city.CityWall.UpgradeCost.UpgradeCost))
                .ForMember(detail => detail.FarmStage, map => map.MapFrom(city => city.Farm.Stage))
                .ForMember(detail => detail.FarmUpgradeCost, map => map.MapFrom(city => city.Farm.UpgradeCost.UpgradeCost))
                .ForMember(detail => detail.LumberStage, map => map.MapFrom(city => city.WoodProduction.Stage))
                .ForMember(detail => detail.LumberUpgradeCost, map => map.MapFrom(city => city.WoodProduction.UpgradeCost.UpgradeCost))
                .ForMember(detail => detail.SilverMineStage, map => map.MapFrom(city => city.SilverProduction.Stage))
                .ForMember(detail => detail.SilverMineUpgradeCost, map => map.MapFrom(city => city.SilverProduction.UpgradeCost.UpgradeCost))
                .ForMember(detail => detail.StoneMineStage, map => map.MapFrom(city => city.StoneProduction.Stage))
                .ForMember(detail => detail.StoneMineUpgradeCost, map => map.MapFrom(city => city.StoneProduction.UpgradeCost.UpgradeCost))
                .ForMember(detail => detail.WarehouseStage, map => map.MapFrom(city => city.Warehouse.Stage))
                .ForMember(detail => detail.WarehouseUpgradeCost, map => map.MapFrom(city => city.Warehouse.UpgradeCost.UpgradeCost))
                .ForMember(detail => detail.LastResourceQueryTime, map => map.MapFrom(city => city.LastResourceQueryTime))
                .ForMember(detail => detail.CityWallMultiplier, map => map.MapFrom(city => city.CityWall.Stage * 0.05 + 1))
                .ForMember(detail => detail.CityWallDefenseValue, map => map.MapFrom(city => (city.CityWall.Stage * 10)))
                .ForMember(detail => detail.MaximumPopulation, map => map.MapFrom(city => (city.Farm.MaxPopulation)))
                .ForMember(detail => detail.MaximumStorage, map => map.MapFrom(city => (city.Warehouse.MaxSilverStorageCapacity)))
                .ForMember(detail => detail.MaximumStorage, map => map.MapFrom(city => (city.Warehouse.MaxSilverStorageCapacity)))
                .ForMember(detail => detail.WoodProduction, map => map.MapFrom(city => (city.WoodProduction.ProductionAmount)))
                .ForMember(detail => detail.StoneProduction, map => map.MapFrom(city => (city.StoneProduction.ProductionAmount)))
                .ForMember(detail => detail.SilverProduction, map => map.MapFrom(city => (city.SilverProduction.ProductionAmount)))
                .ForMember(detail => detail.CastleStage, map => map.MapFrom(city => city.Castle.Stage))
                .ForMember(detail => detail.CastleUpgradeCost, map => map.MapFrom(city => city.Castle.UpgradeCost.UpgradeCost))
                .ForMember(detail => detail.TavernStage, map => map.MapFrom(city => city.Tavern.Stage))
                .ForMember(detail => detail.TavernUpgradeCost, map => map.MapFrom(city => city.Tavern.UpgradeCost.UpgradeCost));
        }
    }
}
