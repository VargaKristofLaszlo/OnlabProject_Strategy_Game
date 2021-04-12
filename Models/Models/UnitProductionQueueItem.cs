using BackEnd.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class UnitProductionQueueItem : Record
    {
        public string UserId { get; set; }
        public string JobId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string UnitName { get; set; }
        public int CityIndex { get; set; }
        public int Amount { get; set; }     
    }
}
