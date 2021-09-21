using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public class CastleDetails
    {
        public int Stage { get; set; }
        public int MaximumCoinCount { get; set; }
        public int AvailableCoinCount { get; set; }
        public Resources CoinCost { get; set; }
        public Resources NobleCost { get; set; }

    }
}
