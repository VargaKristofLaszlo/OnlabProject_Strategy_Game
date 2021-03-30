using Game.Shared.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public enum SelectOptions
    {
        Building = 0,
        Unit = 1
    }

    public class SelectOptionsTransformer
    {
        public static Array GetEnumValues(SelectOptions selector)
        {
            switch (selector)
            {
                case SelectOptions.Building: return Enum.GetValues(typeof(BuildingSelector));
                case SelectOptions.Unit: return Enum.GetValues(typeof(UnitSelector));
                default: return Array.Empty<SelectOptions>();
            }
        }
    }
}
