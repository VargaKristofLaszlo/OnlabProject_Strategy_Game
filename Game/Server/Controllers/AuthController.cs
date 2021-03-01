using BackEnd.Infrastructure;
using BackEnd.Services.Interfaces;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Request;
using Shared.Models.Response;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace Game.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _userService;
        private readonly AuthOptions _authOptions;
        public AuthController(IAuthenticationService userService, AuthOptions authOptions)
        {
            _userService = userService;
            _authOptions = authOptions;
        }
       
        
        [Authorize]
        [HttpPost("Update/Email")]
        [SwaggerOperation(
            Summary = "Send a verification email about the email update",
            Description = "A logged in user can change it's email address" +
            "Using this end-point send an email to the old address to confirm the change." +
            "<b>Using this end-point requires the user to log in<b>",
            Tags = new[] { "User management" }
        )]
        [SwaggerResponse(500, "The email update failed")]
        [SwaggerResponse(404, "The user could not be found")]
        [SwaggerResponse(200, "The email-update confirmation was sent to the user's email address")]
        public async Task<IActionResult> UpdateEmail([FromForm] EmailUpdateRequest request)
        {
            await _userService.SendEmailUpdateConfirmationAsync(request.newEmail);

            return Ok();
        }

        [Authorize]
        [HttpPost("Update/Password")]
        [SwaggerOperation(
            Summary = "Modifies the user's password",
            Description = "A loggedi n user can change it's password." +
            "<b>Using this end-point requires the user to log in<b>",
            Tags = new[] { "User management" }
        )]
        [SwaggerResponse(500, "Password update failed")]
        [SwaggerResponse(204, "The password update was successful")]
        public async Task<IActionResult> UpdatePassword([FromForm] PasswordUpdateRequest request)
        {
            await _userService.UpdatePasswordAsync(request.CurrentPassword, request.NewPassword);

            return NoContent();
        }


       
        [HttpGet("VerifyEmailUpdate")]
        [SwaggerOperation(
            Summary = "Confirms the email modification",
            Description = "The user used the email sent to it's address to confirm the change of the address",
            Tags = new[] { "User management" }
        )]
        [SwaggerResponse(404, "The user could not be found")]
        [SwaggerResponse(500, "The address modification failed")]
        public async Task<IActionResult> ConfirmEmailUpdate([FromQuery] string username, [FromQuery] string email, [FromQuery] string token)
        {
            await _userService.UpdateEmailAddressAsync(username, email, token);

            return Redirect($"{_authOptions.AppUrl}/confirmemail.html");
        }

       
        [Authorize]
        [HttpPost("Delete/Confirmation")]
        [SwaggerOperation(
            Summary = "Sends a verification email about the account deletion",
            Description = "Send an email to the logged in user's email address to confirm the deletion of it's account." +
            "<b>Using this end-point requires the user to log in<b>",
            Tags = new[] { "User management" }
        )]
        [SwaggerResponse(200, "The verification email was sent successfully")]
        [SwaggerResponse(404, "The user could not be found")]
        public async Task<IActionResult> SendDeleteConfirmation()
        {
            await _userService.SendAccountDeletionConfirmationAsync();
            return Ok();
        }


        
        [HttpGet("Delete/Action")]
        [SwaggerOperation(
            Summary = "Deletes the user's account",
            Description = "The user used the verification email to delete it's account",
            Tags = new[] { "User management" }
        )]
        [SwaggerResponse(404, "The user could not be found")]
        public async Task<IActionResult> ConfirmDelete([FromQuery] string username, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(username) ||string.IsNullOrEmpty(token))
                return NotFound();            
            await _userService.DeleteAccountAsync(username, token);

            return Redirect($"{_authOptions.AppUrl}/confirmemail.html");
        }

    }
}
