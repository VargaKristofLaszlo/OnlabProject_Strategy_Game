using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public class UnitQueue
    {
        public List<UnitQueueData> Queue { get; set; } = new List<UnitQueueData>();
    }
    public class UnitQueueData 
    {
        public DateTime CreationTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string UnitName { get; set; }
        public int Amount { get; set; }
        public int CityIndex { get; set; }
        public TimeSpan RecruitTime { get; set; }
        public string JobId { get; set; }
    }
  
}
