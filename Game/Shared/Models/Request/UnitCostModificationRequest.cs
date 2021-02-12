using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models.Request
{
    public class UnitCostModificationRequest
    {
        public string Name { get; set; }
        public int Wood { get; set; }
        public int Stone { get; set; }
        public int Silver { get; set; }
        public int Population { get; set; }
    }
}
