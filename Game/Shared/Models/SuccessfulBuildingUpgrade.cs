using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public record SuccessfulBuildingStageModification
    {
        public Resources NewUpgradeCost { get; init; }
        public int NewStage { get; init; }
    }
}
