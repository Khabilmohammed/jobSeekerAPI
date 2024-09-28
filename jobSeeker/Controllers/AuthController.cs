
using Azure;
using Azure.Identity;
using jobSeeker.DataAccess.Data.Repository.IUserRepository;
using jobSeeker.DataAccess.Services.IEmailService;
using jobSeeker.DataAccess.Services.IUserRepositoryService;
using jobSeeker.DataAccess.Services.OtpService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using jobSeeker.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static System.Net.WebRequestMethods;

namespace jobSeeker.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Register request");
                return BadRequest(ResponseHelper.Error(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            }

            try
            {
                await _authService.RegisterAsync(model);
                _logger.LogInformation("User otp sent successfully. OTP sent to email: {Email}", model.Email);
                return Ok(new { Message = "OTP has been sent to your email." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Login request");
                return BadRequest(ResponseHelper.Error(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }
            try
            {
                var response = await _authService.LoginAsync(model.Email,model.Password);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _logger.LogInformation("User logged in successfully: {Email}", model.Email);
                    return Ok(ResponseHelper.Success(response));
                }
                else
                {
                    _logger.LogWarning("Failed login attempt for user: {Email}", model.Email);
                    return StatusCode((int)response.StatusCode, ResponseHelper.Error(response.ErrorMessages, response.StatusCode));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Email}", model.Email);
                return Unauthorized(new { Error = ex.Message });
            }
        }

        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOTP([FromBody] OTPValidationRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for otp validation");
                return BadRequest(ModelState);
            }

            try
            {
                var success = await _authService.ValidateOtpAsync(model);
                if (success)
                {
                    _logger.LogInformation("OTP validated successfully for user: {Email}", model.Email);
                    return Ok(new { Message = "OTP validated and user registered successfully." });
                }
                else
                {
                    _logger.LogWarning("ivalid otp registration for user:{Email}",model.Email); 
                    return BadRequest(new { Error = "Invalid OTP or registration details." });

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during the otp validation");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Logout attempt failed: Invalid or missing token.");
                return BadRequest(ResponseHelper.Error("Invalid or missing token."));
            }

            try
            {
                await _authService.LogoutAsync(token);
                _logger.LogInformation("User logged out successfully with token: {Token}", token);
                return Ok(ResponseHelper.Success(new { Message = "User logged out successfully." }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout for token: {Token}", token);
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }
    }
}
