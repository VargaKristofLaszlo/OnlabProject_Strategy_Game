using AutoMapper;
using Game.Shared.Models;
using Models.Models;


namespace Models.Profiles
{
    public class BuildingQueueProfile : Profile
    {
        public BuildingQueueProfile()
        {
            CreateMap<HangFireJob, QueueData>()
                .ForMember(s => s.BuildingName, m => m.MapFrom(d => d.BuildingName))
                .ForMember(s => s.CityIndex, m => m.MapFrom(d => d.CityIndex))
                .ForMember(s => s.CreationTime, m => m.MapFrom(d => d.CreationTime))
                .ForMember(s => s.FinishTime, m => m.MapFrom(d => d.FinishTime))
                .ForMember(s => s.NewStage, m => m.MapFrom(d => d.NewStage));
        }
    }
}
