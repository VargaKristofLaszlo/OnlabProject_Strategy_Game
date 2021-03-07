using Game.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBuildingService
    {
        Task<SuccessfulBuildingStageModification> UpgradeBuilding(int cityIndex, string buildingName, int newStage);
        Task<SuccessfulBuildingStageModification> DowngradeBuilding(int cityIndex, string buildingName, int newStage);
    }
}
