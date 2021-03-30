using AutoMapper;
using Game.Shared.Models;
using Game.Shared.Models.Request;
using System;

namespace Models.Profiles
{
    public class BuildingUpgradeCostProfile : Profile
    {
        public BuildingUpgradeCostProfile()
        {
            CreateMap<BackEnd.Models.Models.BuildingUpgradeCost, BuildingUpgradeCost>()
                .ForMember(dto => dto.Population, model => model.MapFrom(cost => cost.UpgradeCost.Population))
                .ForMember(dto => dto.Silver, model => model.MapFrom(cost => cost.UpgradeCost.Silver))
                .ForMember(dto => dto.Stone, model => model.MapFrom(cost => cost.UpgradeCost.Stone))
                .ForMember(dto => dto.Wood, model => model.MapFrom(cost => cost.UpgradeCost.Wood));


            CreateMap<UpgradeCostCreationRequest, BackEnd.Models.Models.BuildingUpgradeCost>()
                .ForMember(dto => dto.BuildingName, model => model.MapFrom(cost => cost.BuildingName))
                .ForMember(dto => dto.BuildingStage, model => model.MapFrom(cost => cost.BuildingStage))
                .ForMember(dto => dto.UpgradeTime, model => model.MapFrom(cost => new TimeSpan(0, 0, cost.UpgradeTimeInSeconds)))
                .ForMember(dto => dto.UpgradeCost, model => model.MapFrom(cost => cost.UpgradeCost));



            CreateMap<BackEnd.Models.Models.BuildingUpgradeCost, UpgradeCostCreationRequest>()
                .ForMember(s => s.UpgradeCost, m => m.MapFrom(t => t.UpgradeCost))
                .ForMember(s => s.UpgradeTimeInSeconds, m => m.MapFrom(t => t.UpgradeTime.TotalSeconds))
                .ForMember(s => s.BuildingName, m => m.MapFrom(t => t.BuildingName))
                .ForMember(s => s.BuildingStage, m => m.MapFrom(t => t.BuildingStage));

        }        
    }
}
