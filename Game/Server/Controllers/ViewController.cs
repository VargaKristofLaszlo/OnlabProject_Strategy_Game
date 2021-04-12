using Game.Shared.Models;
using Game.Shared.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Queries;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ViewController : ControllerBase
    {
       
        private readonly IMediator _mediator;

        public ViewController(IMediator mediator)
        {           
            _mediator = mediator;
        }

        [Authorize(Policy = "AdminOnly")]
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
            var response = await _mediator.Send(new GetUserCredentials.Query(pageNumber, pageSize));
            return Ok(response);
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
            var response = await _mediator.Send(new GetBuildingUpgradeCost.Query(buildingName, buildingStage));
            return Ok(response);
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
        [SwaggerResponse(200, "The request was successful", typeof(IEnumerable<string>))]
        public async Task<IActionResult> GetCityNamesOfLoggedInUser()
        {
            var response = await _mediator.Send(new GetCityNamesOfUser.Query());
            return Ok(response);
        }

        [HttpGet("CityNames/{id}")]
        [SwaggerOperation(
          Summary = "Returns a list containing the name of the cities",
          Description = "Returns a list containing the name of the cities." +
          "Can be used to either list the names for the admin or in-game for a user" +
          "<b>Using this end-point requires the user to log in<b>"
      )]
        [SwaggerResponse(404, "The user could not be found")]
        [SwaggerResponse(401, "Only a logged in user can use this endpoint")]
        [SwaggerResponse(200, "The request was successful", typeof(IEnumerable<string>))]
        public async Task<IActionResult> GetCityNamesOfUserById(string id)
        {
            var response = await _mediator.Send(new GetCityNamesOfUser.Query(id));
            return Ok(response);
        }


        [HttpGet("Units")]
        [SwaggerOperation(
            Summary = "Returns a list containing informations about the unit types",
            Description = "Returns a list containing informations about the unit types via paging" +
            "<b>Using this end-point requires the user to log in<b>"
        )]
        [SwaggerResponse(200, "The request was successful",typeof(CollectionResponse<Shared.Models.Unit>))]
        [SwaggerResponse(401, "Only a logged in user can use this endpoint")]
        public async Task<IActionResult> GetUnitTypes ([FromQuery] int pageNumber, [FromQuery] int pageSize) 
        {
            var response = await _mediator.Send(new GetUnitTypes.Query(pageNumber, pageSize));
            return Ok(response);
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
            var response =  await _mediator.Send(new GetCityDetails.Query(cityIndex));
            return Ok(response);
        }


        [HttpGet("Units/{unitName}")]
       
        public async Task<IActionResult> GetUnitByName(string unitName) 
        {
            var response = await _mediator.Send(new GetUnitByName.Query(unitName));
            return Ok(response);
        }
        [HttpGet("Building/{buildingName}/Upgradecost")]

        public async Task<IActionResult> GetBuildingUpgradeCosts(string buildingName)
        {
            var response = await _mediator.Send(new GetBuildingUpgradeCostsByName.Query(buildingName));
            return Ok(response);
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
            var response = await _mediator.Send(new GetResourcesOfCity.Query(cityIndex));
            return Ok(response);
        }

        [HttpGet("Warehouse/Capacity")]
        public async Task<IActionResult> GetWarehouseCapacity([FromQuery]int cityIndex) 
        {
            var response = await _mediator.Send(new GetWarehouseCapacity.Query(cityIndex));
            return Ok(response);
        }

        [HttpGet("UnitsOfCity")]
        public async Task<IActionResult> GetUnitsOfCity([FromQuery] int cityIndex) 
        {
            var response = await _mediator.Send(new GetUnitsOfCity.Query(cityIndex));
            return Ok(response);
        }

        [HttpGet("Users/Others/Cities")]
        public async Task<IActionResult> GetCitiesOfOtherUsers([FromQuery] int pageNumber, [FromQuery] int pageSize) 
        {
            var response = await _mediator.Send(new GetOtherUsersCities.Query(pageNumber, pageSize));
            return Ok(response);
        }


        [HttpGet("Reports")]
        public async Task<IActionResult> GetReports([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var response = await _mediator.Send(new GetReports.Query(pageNumber, pageSize));
            return Ok(response);
        }

        [HttpGet("BuildingQueue/{userId}")]
        public async Task<IActionResult> GetBuildingQueue(string userId) 
        {
            var response = await _mediator.Send(new GetBuildingQueueByUserID.Query(userId));

            return Ok(response);
        }
        [HttpGet("UnitRecruitQueue/{userId}")]
        public async Task<IActionResult> GetUnitRecruitQueue(string userId)
        {
            var response = await _mediator.Send(new GetUnitRecruitQueueByUserId.Query(userId));

            return Ok(response);
        }
    }
}
