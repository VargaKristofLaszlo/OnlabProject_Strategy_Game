using BackEnd.Services.Interfaces;
using Game.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.Response;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Collections.Generic;
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
        [SwaggerOperation(
            Summary = "Gets the credentials of the users for a page",
            Description = "Returns the informations about the registered users using paging." +
            "<b>Using this end-point requires the user to log in as an admin<b>"
        )]
        [SwaggerResponse(200, "The request was successful",typeof(CollectionResponse<Credentials>))]
        public async Task<IActionResult> GetAllCredentials([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _viewService.GetUserCredentialsAsync(pageNumber, pageSize);
            return Ok(result);
        }

        
        [HttpGet("Building/UpgradeCost")]
        [SwaggerOperation(
            Summary = "Gets the upgrade cost of a building",
            Description = "Returns the upgrade cost of a building." +
            "<b>Using this end-point requires the user to log in<b>"            
        )]
        [SwaggerResponse(404, "The upgrade cost could not be found")]
        [SwaggerResponse(200, "The request was successful",typeof(BuildingUpgradeCost))]
        public async Task<IActionResult> GetBuildingUpgradeCost([FromQuery] string buildingName, [FromQuery] int buildingStage)
        {
            var result = await _viewService.GetBuildingUpgradeCost(buildingName, buildingStage);
            return Ok(result);
        }

       
        [HttpGet("{username}")]
        [SwaggerOperation(
            Summary = "Gets the name of the cities",
            Description = "Returns the names of the cities of a user." +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(404, "The user could not be found")]
        [SwaggerResponse(200, "The request was successful", typeof(CollectionResponse<string>))]
        public async Task<IActionResult> GetCityNamesOfUser(string username, [FromQuery] int pageNumber, [FromQuery] int pageSize) 
        {
            var result = await _viewService.GetCityNamesOfUser(username, pageNumber, pageSize);

            return Ok(result);
        }

        
        [HttpGet("Units")]
        [SwaggerOperation(
            Summary = "Gets the unit types",
            Description = "Returns the informations about the unit types using paging." +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(200, "The request was successful",typeof(CollectionResponse<Unit>))]
        public async Task<IActionResult> GetUnitTypes ([FromQuery] int pageNumber, [FromQuery] int pageSize) 
        {
            var result = await _viewService.GetUnitTypes(pageNumber, pageSize);
            return Ok(result);
        }

        
        [HttpGet("City")]
        [SwaggerOperation(
            Summary = "Returns information about a city",
            Description = "Returns the buildings and their stages and upgrade costs of a user's city." +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(404, "The city could not be found")]
        [SwaggerResponse(200, "The request was successful", typeof(CityDetails))]
        public async Task<IActionResult> GetCity([FromQuery] int cityIndex) 
        {
            var result = await _viewService.GetCityDetails(cityIndex);
            return Ok(result);
        }


        [HttpGet("Units/Producible")]
        [SwaggerOperation(
            Summary = "Returns the producible units",
            Description = "Returns the producible units of the user's city." +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(404, "The city could not be found")]
        [SwaggerResponse(200, "The request was successful", typeof(IEnumerable<Unit>))]
        public async Task<IActionResult> GetProducibleUnits([FromQuery] int cityIndex) 
        {
            var result = await _viewService.GetProducibleUnitTypes(cityIndex);
            return Ok(result);
        }
    
        //Call this when we need to refresh the resources of the city
        [HttpGet("City/Resources")]
        [SwaggerOperation(
            Summary = "Returns the resources of the city",
            Description = "Returns the updated resources of the city." +
            "<b>Call this method when the resources of the city needs to be refreshed<b>." +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(404, "The city could not be found")]
        [SwaggerResponse(200, "The request was successful", typeof(CityResources))]
        public async Task<IActionResult> Test([FromQuery] int cityIndex) 
        {
            var result = await _viewService.GetResourcesOfCity(cityIndex);

            return Ok(result);
        }
    }
}
