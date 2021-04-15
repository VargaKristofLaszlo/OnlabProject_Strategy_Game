using BackEnd.Infrastructure;
using MediatR;
using System;

namespace Services.Notifications
{
    public record UpgradeNotification : INotification
    {
        public string UserId { get; init; }
        public int CityIndex { get; init; }
        public int NewStage { get; init; }
        public string BuildingName { get; init; }
        public DateTime StartTime { get; set; }
    }
}
