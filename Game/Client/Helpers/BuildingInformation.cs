using Game.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public class BuildingInformation
    {
        public string Header { get; set; }
        public string Description { get; set; }
        public object Content { get; set; }
        public Resources UpgradeCost { get; set; }
        public string ImgSource { get; set; }
    }
}
