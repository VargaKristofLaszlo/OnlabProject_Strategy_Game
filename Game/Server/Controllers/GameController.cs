using Game.Shared.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Game.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IBuildingService _buildingService;
        private readonly IAttackService _attackService;

        public GameController(IGameService gameService, IBuildingService buildingService, IAttackService attackService)
        {
            _gameService = gameService;
            _buildingService = buildingService;
            _attackService = attackService;
        }

       
        [HttpPatch("{buildingName}/Upgrade")]
        [SwaggerOperation(
            Summary = "Upgrade a building",
            Description = "Find the city where the building needs to be upgraded using the index of the city " +
            "and find the building which needs to upgraded using it's name." +
            "<b>Using this end-point requires the user to log in<b>"            
        )]
        [SwaggerResponse(200, "The upgrade was successful")]        
        [SwaggerResponse(400, "The upgrade failed")]
        [SwaggerResponse(401, "Only a logged in user can use this endpoint")]
        [SwaggerResponse(404, "The building was not found")]
        public async Task<IActionResult> UpgradeBuilding([FromQuery] int cityIndex, string buildingName, [FromQuery] int newStage) 
        {
            var res = await _buildingService.UpgradeBuilding(cityIndex, buildingName, newStage);
            return Ok(res);
        }

        [HttpPatch("{buildingName}/Downgrade")]
        [SwaggerOperation(
            Summary = "Downgrade a building",
            Description = "Find the city where the building needs to be downgraded using the index of the city " +
            "and find the building which needs to downgraded using it's name." +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(200, "The downgrade was successful")]
        [SwaggerResponse(400, "The downgrade failed")]
        [SwaggerResponse(401, "Only a logged in user can use this endpoint")]
        [SwaggerResponse(404, "The building was not found")]        
        public async Task<IActionResult> DowngradeBuilding([FromQuery] int cityIndex, string buildingName, [FromQuery] int newStage)
        {
            var res = await _buildingService.DowngradeBuilding(cityIndex, buildingName, newStage);
            return Ok(res);
        }

        [HttpPost("ProduceUnit")]
        [SwaggerOperation(
            Summary = "Produces units in the city",
            Description = "Finds the city where to units need to be produced, checks if the city has enough resources, then produces the units." +
            "<b>Using this end-point requires the user to log in<b>"
        )]        
        [SwaggerResponse(200, "The unit production was successful")]
        [SwaggerResponse(400, "The unit production failed")]
        [SwaggerResponse(401, "Only a logged in user can use this endpoint")]
        [SwaggerResponse(404, "The unit type could not be found")]
        public async Task<IActionResult> ProduceUnits([FromBody] UnitProductionRequest request) 
        {
            await _gameService.ProduceUnits(request);
            return Ok();
        }

        [HttpPost("Resources/Send")]
        [SwaggerOperation(
            Summary = "Sends resources to an other user",
            Description = "Takes the resources from the sender's city and gives them to the receiver"
        )]
        [SwaggerResponse(200, "The resources were sent to the other player")]
        [SwaggerResponse(401, "Only a logged in user can use this endpoint")]
        [SwaggerResponse(400, "The resources could not be sent to the other player")]
        public async Task<IActionResult> SendResourcesToOtherPlayer([FromBody] SendResourceToOtherPlayerRequest request) 
        {
            await _gameService.SendResourcesToOtherPlayer(request);
            return Ok();
        }



        [HttpPost("Attack")]
        public async Task<IActionResult> AttackOtherCity([FromBody] AttackRequest request) 
        {
            await _attackService.AttackOtherCity(request);
            return Ok();
        }
    }
}
