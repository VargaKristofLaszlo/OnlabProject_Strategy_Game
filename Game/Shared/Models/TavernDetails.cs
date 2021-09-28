using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public class TavernDetails
    {
        public int Stage { get; set; }
        public int MaximumSpyCount { get; set; }
        public int AvailableSpyCount { get; set; }
        public Resources SpyCost { get; set; }
    }
}
