using BackEnd.Infrastructure;
using BackEnd.Services.Interfaces;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Request;
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


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var loginResult = await _userService.LoginAsync(request);

            return Ok(loginResult);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserCreationRequest request)
        {
            await _userService.RegisterUserAsync(request);
            return Ok();
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userid, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(token))
                return NotFound();

            await _userService.ConfirmEmailAsync(userid, token);

            return Redirect($"{_authOptions.AppUrl}/confirmemail.html");
        }


        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            await _userService.ForgetPasswordAsync(email);

            return Ok();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] PasswordResetRequest request)
        {
            await _userService.ResetPasswordAsync(request);

            return Ok();
        }

        [Authorize]
        [HttpPost("Update/Email")]
        public async Task<IActionResult> UpdateEmail([FromForm] EmailUpdateRequest request)
        {
            await _userService.SendEmailUpdateConfirmationAsync(request.newEmail);

            return Ok();
        }

        [Authorize]
        [HttpPost("Update/Password")]
        public async Task<IActionResult> UpdatePassword([FromForm] PasswordUpdateRequest request)
        {
            await _userService.UpdatePasswordAsync(request.CurrentPassword, request.NewPassword);

            return Ok();
        }

        [HttpGet("VerifyEmailUpdate")]
        public async Task<IActionResult> ConfirmEmailUpdate([FromQuery] string username, [FromQuery] string email, [FromQuery] string token)
        {
            await _userService.UpdateEmailAddressAsync(username, email, token);

            return Redirect($"{_authOptions.AppUrl}/confirmemail.html");
        }

        [Authorize]
        [HttpPost("Delete/Confirmation")]
        public async Task<IActionResult> SendDeleteConfirmation()
        {
            await _userService.SendAccountDeletionConfirmationAsync();

            return Ok();
        }


        [HttpGet("Delete/Action")]
        public async Task<IActionResult> ConfirmDelete([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
                return NotFound();

            await _userService.DeleteAccountAsync(username);

            return Redirect($"{_authOptions.AppUrl}/confirmemail.html");
        }

    }
}
