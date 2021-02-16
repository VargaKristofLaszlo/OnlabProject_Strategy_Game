using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Game.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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

        
        [HttpGet("Building/UpgradeCost")]
        public async Task<IActionResult> GetBuildingUpgradeCost([FromQuery] string buildingName, [FromQuery] int buildingStage)
        {
            var result = await _viewService.GetBuildingUpgradeCost(buildingName, buildingStage);

            return Ok(result);
        }

       
        [HttpGet("{username}")]
        public async Task<IActionResult> GetCityNamesOfUser(string username, [FromQuery] int pageNumber, [FromQuery] int pageSize) 
        {
            var result = await _viewService.GetCityNamesOfUser(username, pageNumber, pageSize);

            return Ok(result);
        }

        
        [HttpGet("Units")]
        public async Task<IActionResult> GetUnitTypes ([FromQuery] int pageNumber, [FromQuery] int pageSize) 
        {
            var result = await _viewService.GetUnitTypes(pageNumber, pageSize);

            return Ok(result);
        }

        
        [HttpGet("City")]
        public async Task<IActionResult> GetCity([FromQuery] int cityIndex) 
        {
            var result = await _viewService.GetCityDetails(cityIndex);

            return Ok(result);
        }


        [HttpGet("Units/Producible")]
        public async Task<IActionResult> GetProducibleUnits([FromQuery] int cityIndex) 
        {
            var result = await _viewService.GetProducibleUnitTypes(cityIndex);

            return Ok(result);
        }
    
        //Call this when we need to refresh the resources of the city
        [HttpGet("City/Resources")]
        public async Task<IActionResult> Test([FromQuery] int cityIndex) 
        {
            var result = await _viewService.GetResourcesOfCity(cityIndex);

            return Ok(result);
        }
    }
}
