using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models.Request
{
    public class SendResourceToOtherPlayerRequest
    {
        public int Wood { get; set; }
        public int Silver { get; set; }
        public int Stone { get; set; }

        public int FromCityIndex { get; set; }
        public string ToUserName { get; set; }
        public int ToCityIndex { get; set; }
    }
}
