using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models.Request
{
    public class UnitCostModificationRequest
    {
        
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The Wood cost {0} must be greater than {1}.")]
        public int Wood { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The Stone cost {0} must be greater than {1}.")]
        public int Stone { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The Silver cost {0} must be greater than {1}.")]
        public int Silver { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The Population cost {0} must be greater than {1}.")]
        public int Population { get; set; }
    }
}
