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
                .ForMember(detail => detail.WarehouseUpgradeCost, map => map.MapFrom(city => city.Warehouse.UpgradeCost.UpgradeCost));
        }
    }
}
