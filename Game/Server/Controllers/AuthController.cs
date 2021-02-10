using BackEnd.Infrastructure;
using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Request;
using Shared.Models.Response;
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
            if (ModelState.IsValid)
            {
                var loginResult = await _userService.LoginAsync(request);
                return Ok(loginResult);
            }
            else
                return BadRequest(LoginResponse.Failed("Some properties are not valid"));


        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserCreationRequest request)
        {
            if (ModelState.IsValid)
            {
                var registerResult = await _userService.RegisterUserAsync(request);

                if (registerResult.IsSuccess)
                {
                    return Ok(registerResult);
                }

                return BadRequest(registerResult);
            }
            return BadRequest(OperationResponse<string>.Failed("Some properties are not valid"));
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userid, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(token))
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(userid, token);

            if (result.IsSuccess)
            {
                return Redirect($"{_authOptions.AppUrl}/confirmemail.html");
            }

            return BadRequest(result);
        }


        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var result = await _userService.ForgetPasswordAsync(email);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] PasswordResetRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ResetPasswordAsync(request);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [Authorize]
        [HttpPost("Update/Email")]
        public async Task<IActionResult> UpdateEmail([FromForm] EmailUpdateRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.SendEmailUpdateConfirmationAsync(request.newEmail);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [Authorize]
        [HttpPost("Update/Password")]
        public async Task<IActionResult> UpdatePassword([FromForm] PasswordUpdateRequest request)
        {
            if (ModelState.IsValid)
            {               
                var result = await _userService.UpdatePasswordAsync(request.CurrentPassword, request.NewPassword);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [HttpGet("VerifyEmailUpdate")]
        public async Task<IActionResult> ConfirmEmailUpdate([FromQuery] string username, [FromQuery] string email, [FromQuery] string token)
        {

            var result = await _userService.UpdateEmailAddressAsync(username, email, token);

            if (result.IsSuccess)
            {
                return Redirect($"{_authOptions.AppUrl}/confirmemail.html");
            }

            return BadRequest(result);

        }

        [Authorize]
        [HttpPost("Delete/Confirmation")]
        public async Task<IActionResult> SendDeleteConfirmation()
        {
            var result = await _userService.SendAccountDeletionConfirmationAsync();

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }


        [HttpGet("Delete/Action")]
        public async Task<IActionResult> ConfirmDelete([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username) == false)
            {
                var result = await _userService.DeleteAccountAsync(username);

                if (result.IsSuccess)
                    return Redirect($"{_authOptions.AppUrl}/confirmemail.html");

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

    }
}
