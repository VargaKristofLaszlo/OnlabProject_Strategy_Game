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
               .ForMember(backendModel => backendModel.Name, map => map.MapFrom(sharedModel => sharedModel.Name))
               .ForMember(backendModel => backendModel.AttackPoint, map => map.MapFrom(sharedModel => sharedModel.AttackPoint))
               .ForMember(backendModel => backendModel.InfantryDefensePoint, map => map.MapFrom(sharedModel => sharedModel.InfantryDefensePoint))
               .ForMember(backendModel => backendModel.CavalryDefensePoint, map => map.MapFrom(sharedModel => sharedModel.CavalryDefensePoint))
               .ForMember(backendModel => backendModel.ArcherDefensePoint, map => map.MapFrom(sharedModel => sharedModel.ArcherDefensePoint))
               .ForMember(backendModel => backendModel.CarryingCapacity, map => map.MapFrom(sharedModel => sharedModel.CarryingCapacity))
               .ForMember(backendModel => backendModel.MinBarrackStage, map => map.MapFrom(sharedModel => sharedModel.MinBarrackStage))
               .ForMember(backendModel => backendModel.UnitCost, map => map.MapFrom(sharedModel => sharedModel.UnitCost))
               .ForMember(backendModel => backendModel.UnitType, map =>map.MapFrom(sharedModel => sharedModel.UnitType))
               .ReverseMap();  
        }
    }
}
