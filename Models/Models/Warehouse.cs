using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Models.Models
{
    public class Warehouse : BuildingRecord
    {
        public int MaxSilverStorageCapacity { get; set; }
        public int MaxStoneStorageCapacity { get; set; }
        public int MaxWoodStorageCapacity { get; set; }
    }
}
