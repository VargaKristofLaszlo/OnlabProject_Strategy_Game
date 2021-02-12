using AutoMapper;
using System;

namespace Models.Profiles
{
    public class BuildingUpgradeCostProfile : Profile
    {
        public BuildingUpgradeCostProfile()
        {
            CreateMap<BackEnd.Models.Models.BuildingUpgradeCost, Shared.Models.BuildingUpgradeCost>()
                .ForMember(dto => dto.Population, model => model.MapFrom(cost => cost.UpgradeCost.Population))
                .ForMember(dto => dto.Silver, model => model.MapFrom(cost => cost.UpgradeCost.Silver))
                .ForMember(dto => dto.Stone, model => model.MapFrom(cost => cost.UpgradeCost.Stone))
                .ForMember(dto => dto.Wood, model => model.MapFrom(cost => cost.UpgradeCost.Wood));


            CreateMap<Shared.Models.Request.UpgradeCostCreationRequest, BackEnd.Models.Models.BuildingUpgradeCost>()
                .ForMember(dto => dto.BuildingName, model => model.MapFrom(cost => cost.BuildingName))
                .ForMember(dto => dto.BuildingStage, model => model.MapFrom(cost => cost.BuildingStage))
                .ForMember(dto => dto.UpgradeTime, model => model.MapFrom(cost => new TimeSpan(0, 0, cost.UpgradeTimeInSeconds)))
                .ForMember(dto => dto.UpgradeCost, model => model.MapFrom(cost => cost.UpgradeCost));
               

        }        
    }
}
