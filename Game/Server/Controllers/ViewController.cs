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
            Summary = "Returns a list containing user informations for the admins",
            Description = "Returns the informations about the registered users via paging." +
            "<b>Using this end-point requires the user to log in as an admin<b>"
        )]
        [SwaggerResponse(200, "The request was successful",typeof(CollectionResponse<Credentials>))]
        [SwaggerResponse(403, "Only a logged in admin user can use this endpoint")]
        [SwaggerResponse(401, "Only a logged in admin user can use this endpoint")]
        public async Task<IActionResult> GetAllCredentials([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _viewService.GetUserCredentialsAsync(pageNumber, pageSize);
            return Ok(result);
        }

        
        [HttpGet("Building/UpgradeCost")]
        [SwaggerOperation(
            Summary = "Returns the upgrade cost of a building",
            Description = "Returns the upgrade cost of a building." +
            "Can be used to get an upgrade cost selecting the name/stage from a list." +
            "<b>Using this end-point requires the user to log in<b>"            
        )]
        [SwaggerResponse(404, "The upgrade cost could not be found")]
        [SwaggerResponse(200, "The request was successful",typeof(BuildingUpgradeCost))]
        [SwaggerResponse(401, "Only a logged in user can use this endpoint")]
        public async Task<IActionResult> GetBuildingUpgradeCost([FromQuery] string buildingName, [FromQuery] int buildingStage)
        {
            var result = await _viewService.GetBuildingUpgradeCost(buildingName, buildingStage);
            return Ok(result);
        }

       
        [HttpGet("CityNames")]
        [SwaggerOperation(
            Summary = "Returns a list containing the name of the cities",
            Description = "Returns a list containing the name of the cities." +
            "Can be used to either list the names for the admin or in-game for a user" +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(404, "The user could not be found")]
        [SwaggerResponse(401, "Only a logged in user can use this endpoint")]
        [SwaggerResponse(200, "The request was successful", typeof(CollectionResponse<string>))]
        public async Task<IActionResult> GetCityNamesOfUser() 
        {
            var result = await _viewService.GetCityNamesOfUser();

            return Ok(result);
        }

        
        [HttpGet("Units")]
        [SwaggerOperation(
            Summary = "Returns a list containing informations about the unit types",
            Description = "Returns a list containing informations about the unit types via paging" +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(200, "The request was successful",typeof(CollectionResponse<Unit>))]
        [SwaggerResponse(401, "Only a logged in user can use this endpoint")]
        public async Task<IActionResult> GetUnitTypes ([FromQuery] int pageNumber, [FromQuery] int pageSize) 
        {
            var result = await _viewService.GetUnitTypes(pageNumber, pageSize);
            return Ok(result);
        }

        
        [HttpGet("City")]
        [SwaggerOperation(
            Summary = "Returns information about a city",
            Description = "Usign this endpoint returns the following: <b>Name of the city, Building stages and their upgrade cost," +
            "and the last time the resources of the city was updated.<b>" +
            "This endpoint can be used to get data for a page where you'd want to selec witch building to upgrade." +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(404, "The city could not be found")]
        [SwaggerResponse(401, "Only a logged in user can use this endpoint")]
        [SwaggerResponse(200, "The request was successful", typeof(CityDetails))]
        public async Task<IActionResult> GetCity([FromQuery] int cityIndex) 
        {
            
            var result = await _viewService.GetCityDetails(cityIndex);
            return Ok(result);
        }


        [HttpGet("Units/Producible")]
        [SwaggerOperation(
            Summary = "Returns a list containing information about the producible units",
            Description = "Returns a list containing information about the producible units" +
            "Use this endpoint to get the data for a page where you can select a unit for production" +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(404, "The city could not be found")]
        [SwaggerResponse(401, "Only a logged in user can use this endpoint")]
        [SwaggerResponse(200, "The request was successful", typeof(IEnumerable<Unit>))]
        public async Task<IActionResult> GetProducibleUnits([FromQuery] int cityIndex) 
        {
            var result = await _viewService.GetProducibleUnitTypes(cityIndex);
            return Ok(result);
        }
    
        //Call this when we need to refresh the resources of the city
        [HttpGet("City/Resources")]
        [SwaggerOperation(
            Summary = "Updates the resources of a city",
            Description = "Returns the updated resources of the city." +
            "<b>Call this method when the resources of the city needs to be refreshed<b>." +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(404, "The city could not be found")]
        [SwaggerResponse(401, "Only a logged in user can use this endpoint")]
        [SwaggerResponse(200, "The request was successful", typeof(CityResources))]
        public async Task<IActionResult> ResourceUpdate([FromQuery] int cityIndex) 
        {
            var result = await _viewService.GetResourcesOfCity(cityIndex);

            return Ok(result);
        }

        [HttpGet("Warehouse/Capacity")]
        public async Task<IActionResult> GetWarehouseCapacity([FromQuery]int cityIndex) 
        {
            var result = await _viewService.GetWarehouseCapacity(cityIndex);
            return Ok(result);
        }

        [HttpGet("UnitsOfCity")]
        public async Task<IActionResult> GetUnitsOfCity([FromQuery] int cityIndex) 
        {
            var result = await _viewService.GetUnitsOfCity(cityIndex);
            return Ok(result);
        }
    }
}
