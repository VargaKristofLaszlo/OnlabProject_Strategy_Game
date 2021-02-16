using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public class CityResources
    {
        public int StoneAmount { get; set; }
        public int StoneProductionPerHour { get; set; }        
        public int SilverAmount { get; set; }
        public int SilverProductionPerHour { get; set; }
        public int WoodAmount { get; set; }
        public int WoodProductionPerHour { get; set; }
    }
}
