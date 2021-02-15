using BackEnd.Services.Interfaces;
using Game.Shared.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Request;
using Shared.Models.Response;
using System.Threading.Tasks;

namespace Game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }



        [HttpPost("Ban")]
        public async Task<IActionResult> BanUser([FromBody] UserBanRequest banRequest)
        {
            await _adminService.BanUserAsync(banRequest);

            return NoContent();
        }

        [HttpPost("Create/UpgradeCost")]
        public async Task<IActionResult> CreateBuildingUpgradeCost([FromBody] UpgradeCostCreationRequest request)
        {
            await _adminService.CreateBuildingUpgradeCostAsync(request);

            return NoContent();
        }

        [HttpPut("Modify/UpgradeCost")]
        public async Task<IActionResult> ModifyBuildingUpgradeCost([FromBody] UpgradeCostCreationRequest request) 
        {
            await _adminService.ModifyBuildingUpgradeCostAsync(request);

            return NoContent();
        }

        [HttpPut("Moderate/Cityname")]
        public async Task<IActionResult> ModerateCityName([FromBody] CityNameModerationRequest request) 
        {
            await _adminService.ModerateCityNameAsync(request);

            return NoContent();
        }

        [HttpPut("Modify/UnitCost")]
        public async Task<IActionResult> ModifyUnitCost([FromBody] UnitCostModificationRequest request) 
        {
            await _adminService.ModifyUnitCostAsync(request);

            return NoContent();
        }
    }
}
