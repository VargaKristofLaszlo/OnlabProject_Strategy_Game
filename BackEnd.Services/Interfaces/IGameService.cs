using BackEnd.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IGameService
    {
        Task UpgradeBuilding(int cityIndex, string buildingName, int newStage);
        Task DowngradeBuilding(int cityIndex, string buildingName, int newStage);
    }
}
