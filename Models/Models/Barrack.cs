using System.Collections.Generic;

namespace BackEnd.Models.Models
{
    public class Barrack : BuildingRecord
    {            
        public List<UnitsInCity> UnitsInCity { get; set; } = new List<UnitsInCity>();
    }
}