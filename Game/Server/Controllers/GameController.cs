using Game.Shared.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Implementations;
using Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerOperation(
            Summary = "Upgrade a building",
            Description = "Find the city where the building needs to be upgraded using the index of the city " +
            "and find the building which needs to upgraded using it's name." +
            "<b>Using this end-point requires the user to log in<b>"            
        )]
        [SwaggerResponse(204, "The upgrade was successful")]
        [SwaggerResponse(404, "The building was not found")]
        [SwaggerResponse(400, "The upgrade failed")]
        public async Task<IActionResult> UpgradeBuilding([FromQuery] int cityIndex, string buildingName, [FromQuery] int newStage) 
        {
            await _gameService.UpgradeBuilding(cityIndex, buildingName, newStage);

            return NoContent();
        }

        [HttpPatch("{buildingName}/Downgrade")]
        [SwaggerOperation(
            Summary = "Downgrade a building",
            Description = "Find the city where the building needs to be downgraded using the index of the city " +
            "and find the building which needs to downgraded using it's name." +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(204, "The downgrade was successful")]
        [SwaggerResponse(404, "The building was not found")]
        [SwaggerResponse(400, "The downgrade failed")]
        public async Task<IActionResult> DowngradeBuilding([FromQuery] int cityIndex, string buildingName, [FromQuery] int newStage)
        {
            await _gameService.DowngradeBuilding(cityIndex, buildingName, newStage);

            return NoContent();
        }

        [HttpPost("ProduceUnit")]
        [SwaggerOperation(
            Summary = "Produces units in the city",
            Description = "Finds the city where to units need to be produced, checks if the city has enough resources, then produces the units." +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(404, "The unit type could not be found")]
        [SwaggerResponse(200, "The unit production was successful")]
        [SwaggerResponse(400, "The unit production failed")]
        public async Task<IActionResult> ProduceUnits([FromBody] UnitProductionRequest request) 
        {
            await _gameService.ProduceUnits(request);
            return Ok();
        }

        [HttpPost("Resources/SendTo/{username}")]
        [SwaggerOperation(
            Summary = "Sends resources to an other user",
            Description = "Takes the resources from the sender's city and gives them to the receiver"
        )]
        [SwaggerResponse(200, "The resources were sent to the other player")]
        [SwaggerResponse(400, "The resources could not be sent to the other player")]
        public async Task<IActionResult> SendResourcesToOtherPlayer(string username, [FromBody] SendResourceToOtherPlayerRequest request) 
        {
            await _gameService.SendResourcesToOtherPlayer(request);

            return Ok();
        }

    }
}
