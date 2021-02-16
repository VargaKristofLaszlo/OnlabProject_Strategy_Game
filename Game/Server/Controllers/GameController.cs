using Game.Shared.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Implementations;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

       
        [HttpPatch("{buildingName}/Upgrade")]
        public async Task<IActionResult> UpgradeBuilding([FromQuery] int cityIndex, string buildingName, [FromQuery] int newStage) 
        {
            await _gameService.UpgradeBuilding(cityIndex, buildingName, newStage);

            return NoContent();
        }

        [HttpPatch("{buildingName}/Downgrade")]
        public async Task<IActionResult> DowngradeBuilding([FromQuery] int cityIndex, string buildingName, [FromQuery] int newStage)
        {
            await _gameService.DowngradeBuilding(cityIndex, buildingName, newStage);

            return NoContent();
        }

        [HttpPost("ProduceUnit")]
        public async Task<IActionResult> ProduceUnits([FromBody] UnitProductionRequest request) 
        {
            await _gameService.ProduceUnits(request);

            return Ok();
        }

        [HttpPost("Resources/SendTo/{username}")]
        public async Task<IActionResult> SendResourcesToOtherPlayer(string username, [FromBody] SendResourceToOtherPlayerRequest request) 
        {
            await _gameService.SendResourcesToOtherPlayer(request);

            return Ok();
        }

    }
}
