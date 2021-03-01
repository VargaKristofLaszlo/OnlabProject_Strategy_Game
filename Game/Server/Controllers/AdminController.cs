using BackEnd.Services.Interfaces;
using Game.Shared.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Request;
using Shared.Models.Response;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Game.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
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
            await _adminService.BanUserAsync(banRequest);
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
            await _adminService.CreateBuildingUpgradeCostAsync(request);
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
            await _adminService.ModifyBuildingUpgradeCostAsync(request);
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
            await _adminService.ModerateCityNameAsync(request);
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
            await _adminService.ModifyUnitCostAsync(request);
            return NoContent();
        }
    }
}
