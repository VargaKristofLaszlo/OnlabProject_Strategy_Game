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
        [Post("/cancel/recruitment")]
        Task RemoveRecruitmentFromQueue(string jobId);

        [Patch("/{buildingName}/Upgrade")]
        Task<ApiResponse<string>> UpgradeBuilding(int cityIndex, string buildingName, int newStage);

        [Patch("/{buildingName}/Downgrade")]
        Task<ApiResponse<SuccessfulBuildingStageModification>> DowngradeBuilding(int cityIndex, string buildingName, int newStage);

        [Post("/ProduceUnit")]
        Task<HttpResponseMessage> ProduceUnits([Body] UnitProductionRequest request);

        [Post("/Resources/Send")]
        Task<HttpResponseMessage> SendResourcesToOtherPlayer([Body] SendResourceToOtherPlayerRequest request);

        [Post("/Create/Coins")]
        Task<HttpResponseMessage> CreateCoins([Body] CoinCreationRequest request);

        [Post("/Recruit/Spy")]
        Task<HttpResponseMessage> RecruitSpy([Query] int amount, [Query] int cityIndex);

        [Post("/Spy")]
        Task<ApiResponse<string>> SpyOtherCity([Body] SpyRequest request);

    }
}
