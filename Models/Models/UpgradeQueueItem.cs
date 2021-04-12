using BackEnd.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class UpgradeQueueItem : Record
    {
        public string UserId { get; set; }
        public string JobId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string BuildingName { get; set; }
        public int NewStage { get; set; }
        public int CityIndex { get; set; }
      
    }
}
