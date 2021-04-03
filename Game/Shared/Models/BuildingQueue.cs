using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public class BuildingQueue
    {
        public List<QueueData> Queue { get; set; } = new List<QueueData>();
    }
    public class QueueData 
    {
        public DateTime CreationTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string BuildingName { get; set; }
        public int NewStage { get; set; }
        public int CityIndex { get; set; }
        public TimeSpan UpgradeTime { get; set; }
        public string JobId { get; set; }
    }
}
