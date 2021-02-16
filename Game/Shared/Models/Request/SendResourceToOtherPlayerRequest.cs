using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models.Request
{
    public class SendResourceToOtherPlayerRequest
    {
        public int WoodAmount { get; set; }
        public int SilverAmount { get; set; }
        public int StoneAmount { get; set; }

        public int fromCityIndex { get; set; }
        public string toUserName { get; set; }
        public int toCityIndex { get; set; }
    }
}
