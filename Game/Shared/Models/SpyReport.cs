using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public class SpyReport
    {
        public CityDetails BuildingInformations { get; set; }
        public UnitsOfTheCity UnitsInTheCity { get; set; }
        public bool Successful { get; set; }
    }
}
