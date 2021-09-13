using BackEnd.Infrastructure;
using Game.Shared.Models.Request;
using Hangfire;
using Hangfire.MediatR;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Commands;
using Services.Commands.Buildings;
using Services.Commands.Game;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Game.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("cancel/recruitment")]
        public async Task<IActionResult> CancelRecruitment([FromQuery] string jobId)
        {
            await _mediator.Send(new RemoveRecruitmentJob.Command(jobId));
            var client = new BackgroundJobClient();

            client.Delete(jobId);
            RecurringJob.RemoveIfExists(jobId);

            return Ok();
        }

        [HttpPost("cancel/building/upgrade")]
        public async Task<IActionResult> CancelUpgrade([FromQuery] string jobId)
        {
            await _mediator.Send(new RemoveBuildingUpgradeJob.Command(jobId));
            var client = new BackgroundJobClient();

            client.Delete(jobId);
            RecurringJob.RemoveIfExists(jobId);

            return Ok();
        }

        [HttpPatch("{buildingName}/Upgrade")]
        public async Task<IActionResult> UpgradeBuilding([FromQuery] int cityIndex, string buildingName, [FromQuery] int newStage)
        {
            var identityContext = new IdentityContext(HttpContext);

            var startTime = await _mediator.Send(new UpgradeStart.Command(cityIndex, buildingName, newStage, identityContext));

            var jobId = _mediator.Schedule(
                $"{identityContext.UserId} upgrades {buildingName} to stage {newStage}",
                new UpgradeProcess.Command(cityIndex, buildingName, newStage, identityContext, startTime),
                startTime);

            await _mediator.Send(new AddJobIdToBuildingQueue.Command(jobId, identityContext.UserId, buildingName, cityIndex, newStage));

            return Ok(jobId);
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
            var response = await _mediator.Send(new DowngradeBuilding.Command(cityIndex, buildingName, newStage));
            return Ok(response);
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
            var identityContext = new IdentityContext(HttpContext);
            var startTime = await _mediator.Send(new UnitProductionStart.Command(request, identityContext));

            var jobId = _mediator.Schedule(
               $"{identityContext.UserId} creates {request.Amount} {request.NameOfUnitType}",
               new UnitProductionProcess.Command(request.CityIndex, request.NameOfUnitType, request.Amount, identityContext, startTime),
               startTime);

            await _mediator.Send(new AddJobIdToUnitQueue.Command(jobId, identityContext.UserId, request.NameOfUnitType,
                request.CityIndex, request.Amount));

            return Ok(jobId);
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
            await _mediator.Send(new SendResourcesToOtherPlayer.Command(request));
            return Ok();
        }



        [HttpPost("Attack")]
        public async Task<IActionResult> AttackOtherCity([FromBody] AttackRequest request)
        {
            await _mediator.Send(new AttackOtherCity.Command(request));
            return Ok();
        }

        [HttpPost("Create/Coins")]
        public async Task<IActionResult> CreateCoins([FromBody] CoinCreationRequest request)
        {
            await _mediator.Send(new AddNewCoins.Command(request));
            return Ok();
        }
    }
}
