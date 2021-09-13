using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models.Request
{
    public class CoinCreationRequest
    {
        public int Amount { get; set; }
        public int CityIndex { get; set; }
    }
}
