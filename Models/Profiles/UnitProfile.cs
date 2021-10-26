using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Profiles
{
    public class UnitProfile : Profile
    {
        public UnitProfile()
        {
            CreateMap<BackEnd.Models.Models.Unit, Game.Shared.Models.Unit>()
               .ForMember(sharedModel => sharedModel.Name, map => map.MapFrom(backendModel => backendModel.Name))
               .ForMember(sharedModel => sharedModel.AttackPoint, map => map.MapFrom(backendModel => backendModel.AttackPoint))
               .ForMember(sharedModel => sharedModel.InfantryDefensePoint, map => map.MapFrom(backendModel => backendModel.InfantryDefensePoint))
               .ForMember(sharedModel => sharedModel.CavalryDefensePoint, map => map.MapFrom(backendModel => backendModel.CavalryDefensePoint))
               .ForMember(sharedModel => sharedModel.ArcherDefensePoint, map => map.MapFrom(backendModel => backendModel.ArcherDefensePoint))
               .ForMember(sharedModel => sharedModel.CarryingCapacity, map => map.MapFrom(backendModel => backendModel.CarryingCapacity))
               .ForMember(sharedModel => sharedModel.MinBarrackStage, map => map.MapFrom(backendModel => backendModel.MinBarrackStage))
               .ForMember(sharedModel => sharedModel.UnitCost, map => map.MapFrom(backendModel => backendModel.UnitCost))
               .ForMember(sharedModel => sharedModel.UnitType, map => map.MapFrom(backendModel => backendModel.UnitType))
               .ForMember(sharedModel => sharedModel.RecruitmentTimeInSeconds, map => map.MapFrom(backendModel => backendModel.RecruitTime.TotalSeconds))
               .ReverseMap();
        }
    }
}
