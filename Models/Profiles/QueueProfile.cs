using AutoMapper;
using Game.Shared.Models;
using Models.Models;


namespace Models.Profiles
{
    public class QueueProfile : Profile
    {
        public QueueProfile()
        {
            CreateMap<UpgradeQueueItem, QueueData>()
                .ForMember(s => s.BuildingName, m => m.MapFrom(d => d.BuildingName))
                .ForMember(s => s.CityIndex, m => m.MapFrom(d => d.CityIndex))
                .ForMember(s => s.CreationTime, m => m.MapFrom(d => d.CreationTime))
                .ForMember(s => s.FinishTime, m => m.MapFrom(d => d.FinishTime))
                .ForMember(s => s.JobId, m => m.MapFrom(d => d.JobId))
                .ForMember(s => s.NewStage, m => m.MapFrom(d => d.NewStage));

            CreateMap<UnitProductionQueueItem, UnitQueueData>()
               .ForMember(s => s.UnitName, m => m.MapFrom(d => d.UnitName))
               .ForMember(s => s.CityIndex, m => m.MapFrom(d => d.CityIndex))
               .ForMember(s => s.CreationTime, m => m.MapFrom(d => d.CreationTime))
               .ForMember(s => s.FinishTime, m => m.MapFrom(d => d.FinishTime))
               .ForMember(s => s.JobId, m => m.MapFrom(d => d.JobId))
               .ForMember(s => s.Amount, m => m.MapFrom(d => d.Amount));
        }
    }
}
