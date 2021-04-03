﻿using Game.Shared.Models;
using Game.Shared.Models.Request;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace Game.Shared.IServices
{
    public interface IGameService
    {
        [Post("/cancel/building/upgrade")]
        Task RemoveUpgradeFromQueue(string jobId);

        [Patch("/{buildingName}/Upgrade")]
        Task<ApiResponse<SuccessfulBuildingStageModification>> UpgradeBuilding(int cityIndex, string buildingName, int newStage);

        [Patch("/test/{buildingName}/Upgrade")]
        Task<ApiResponse<string>> TestUpgradeBuilding(int cityIndex, string buildingName, int newStage);

        [Patch("/{buildingName}/Downgrade")]
        Task<ApiResponse<SuccessfulBuildingStageModification>> DowngradeBuilding(int cityIndex, string buildingName, int newStage);

        [Post("/ProduceUnit")]
        Task<HttpResponseMessage> ProduceUnits([Body]UnitProductionRequest request);

        [Post("/Resources/Send")]
        Task<HttpResponseMessage> SendResourcesToOtherPlayer([Body]SendResourceToOtherPlayerRequest request);
        
    }
}
