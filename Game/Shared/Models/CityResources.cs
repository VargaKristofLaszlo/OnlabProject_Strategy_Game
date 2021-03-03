using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public record CityResources
    {
        public int StoneAmount { get; init; }
        public int StoneProductionPerHour { get; init; }        
        public int SilverAmount { get; init; }
        public int SilverProductionPerHour { get; init; }
        public int WoodAmount { get; init; }
        public int WoodProductionPerHour { get; init; }
    }
}
