using Game.Shared.Models.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Commands.Admin;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Game.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {      
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {          
            _mediator = mediator;
        }


        [HttpPost("Ban")]
        [SwaggerOperation(
            Summary = "Bans a specific user",
            Description = "Sending this request prohibits the login process of a specific user." +
            "<b>Sending this request requires an authorized admin user.<b>",
            Tags = new[] { "User management" }
        )]
        [SwaggerResponse(400, "The request model is invalid")]
        [SwaggerResponse(204, "The operation succeeded")]
        [SwaggerResponse(404, "The user could not be found")]
        public async Task<IActionResult> BanUser([FromBody] UserBanRequest banRequest)
        {
            await _mediator.Send(new BanUser.Command(banRequest));
            return NoContent();
        }


        [HttpPost("Create/UpgradeCost")]
        [SwaggerOperation(
            Summary = "Creates a new upgrade cost for a building",
            Description = "Sending this request creates a building upgrade cost entity in the database." +
            "<b>Sending this request requires an authorized admin user.<b>",
            Tags = new[] { "Upgrade cost management" }
        )]
        [SwaggerResponse(205,"The upgrade cost creation succeeded")]
        [SwaggerResponse(400, "The request model is invalid")]
        [SwaggerResponse(500, "The upgrade cost creation failed")]
        [SwaggerResponse(400, "The request model is invalid")]
        public async Task<IActionResult> CreateBuildingUpgradeCost([FromBody] UpgradeCostCreationRequest request)
        {
            await _mediator.Send(new CreateBuildingUpgradeCost.Command(request));
            return StatusCode(205);
        }


        [HttpPut("Modify/UpgradeCost")]
        [SwaggerOperation(
            Summary = "Modifies a stored building's upgrade cost",
            Description = "Sending this request modifies an existing upgrade cost enitity of a building. " +
            "<b>Sending this request requires an authorized admin user.<b>",
            Tags = new[] { "Upgrade cost management" }
        )]
        [SwaggerResponse(204, "The modification was successful")]
        [SwaggerResponse(404, "The upgrade cost was not found")]
        [SwaggerResponse(400, "The request model is invalid")]
        public async Task<IActionResult> ModifyBuildingUpgradeCost([FromBody] UpgradeCostCreationRequest request)
        {
            await _mediator.Send(new ModifyBuildingUpgradeCost.Command(request));
            return NoContent();
        }

      
        [HttpPut("Moderate/Cityname")]
        [SwaggerOperation(
            Summary = "Moderates the name of a city",
            Description = "Sending this requests updates the name of a specific city." +
            "<b>Sending this request requires an authorized admin user.<b>",
            Tags = new[] { "Moderations" }
        )]
        [SwaggerResponse(404, "The city was not found")]
        [SwaggerResponse(204, "The modification was successful")]
        [SwaggerResponse(400, "The request model is invalid")]
        public async Task<IActionResult> ModerateCityName([FromBody] CityNameModerationRequest request)
        {
            await _mediator.Send(new ModerateCityName.Command(request));
            return NoContent();
        }

        [HttpPut("Modify/UnitCost")]
        [SwaggerOperation(
            Summary = "Modifies a stored unit's production cost",
            Description = "Sending this request modifies an existing production cost enitity of a unit type." +
            "<b>Sending this request requires an authorized admin user.<b>",
            Tags = new[] { "Unit cost management" }
        )]
        [SwaggerResponse(204, "The modification was successful")]
        [SwaggerResponse(404, "The production cost was not found")]
        [SwaggerResponse(400, "The request model is invalid")]
        public async Task<IActionResult> ModifyUnitCost([FromBody] UnitCostModificationRequest request)
        {
            await _mediator.Send(new ModifyUnitCost.Command(request));
            return NoContent();
        }
    }
}
