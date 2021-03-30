using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models.Request
{
    public class CityNameModerationRequest
    {
        public string UserId { get; set; }
        public string OldCityName { get; set; }
        public string NewCityName { get; set; }
    }
}
