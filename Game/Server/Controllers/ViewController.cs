using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewController : ControllerBase
    {
        private readonly IViewService _viewService;

        public ViewController(IViewService viewService)
        {
            _viewService = viewService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Credentials")]
        public async Task<IActionResult> GetAllCredentials([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _viewService.GetUserCredentialsAsync(pageNumber, pageSize);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("Building/UpgradeCost")]
        public async Task<IActionResult> GetBuildingUpgradeCost([FromQuery] string buildingName, [FromQuery] int buildingStage)
        {
            var result = await _viewService.GetBuildingUpgradeCost(buildingName, buildingStage);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetCityNamesOfUser(string username, [FromQuery] int pageNumber, [FromQuery] int pageSize) 
        {
            var result = await _viewService.GetCityNamesOfUser(username, pageNumber, pageSize);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("Units")]
        public async Task<IActionResult> GetUnitTypes ([FromQuery] int pageNumber, [FromQuery] int pageSize) 
        {
            var result = await _viewService.GetUnitTypes(pageNumber, pageSize);

            return Ok(result);
        }
    }
}
