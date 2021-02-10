using BackEnd.Services.Interfaces;
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
            var result = await _adminService.BanUserAsync(banRequest);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("Create/UpgradeCost")]
        public async Task<IActionResult> CreateBuildingUpgradeCost([FromBody] UpgradeCostCreationRequest request) 
        {
            if (ModelState.IsValid) 
            {
                var result = await _adminService.CreateBuildingUpgradeCostAsync(request);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest(OperationResponse<string>.Failed("Some properties are not valid"));
        }
    }
}
