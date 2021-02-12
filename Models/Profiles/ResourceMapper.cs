using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Profiles
{
    public class ResourceMapper : Profile
    {

        public ResourceMapper()
        {
            CreateMap<BackEnd.Models.Models.Resources, Game.Shared.Models.Resources>()
              .ForMember(dto => dto.Population, model => model.MapFrom(cost => cost.Population))
              .ForMember(dto => dto.Silver, model => model.MapFrom(cost => cost.Silver))
              .ForMember(dto => dto.Stone, model => model.MapFrom(cost => cost.Stone))
              .ForMember(dto => dto.Wood, model => model.MapFrom(cost => cost.Wood)).ReverseMap();
        }
    }
}
