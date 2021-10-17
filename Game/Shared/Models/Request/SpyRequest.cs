using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models.Request
{
    public class SpyRequest
    {
        public string UserId { get; set; }
        public int TargetCityIndex { get; set; }
        public int OwnerCityIndex { get; set; }
        public int UsedSpyCount { get; set; }
    }
}
