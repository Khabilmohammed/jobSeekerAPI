
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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
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


        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOTP([FromBody] ResendOTPRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for resend OTP request");
                return BadRequest(ResponseHelper.Error(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            try
            {
                await _authService.ResendOtpAsync(model.Email);
                _logger.LogInformation("OTP resent successfully. OTP sent to email: {Email}", model.Email);
                return Ok(new { Message = "OTP has been resent to your email." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during resending OTP");
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO model)
        {
            var response = await _authService.RefreshTokenAsync(model.RefreshToken);
            if (!response.IsSuccess)
                return StatusCode((int)response.StatusCode, response);

            return Ok(response);
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
        [HttpPost("registration-resend-otp")]
        public async Task<IActionResult> RegistrationResendOtp([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required.");

            try
            {
                await _authService.RegistartionResendOtpAsync(email);
                return Ok(new { Message = "New OTP has been sent to your email." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
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

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for forgot password request");
                return BadRequest(ModelState);
            }

            try
            {
                await _authService.ForgetPasswordAsync(model.Email);
                _logger.LogInformation("Password reset link sent successfully to: {Email}", model.Email);
                return Ok(new { Message = "Password reset link has been sent to your email." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during forgot password for: {Email}", model.Email);
                return BadRequest(new { Error = ex.Message });
            }
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for reset password request");
                return BadRequest(ModelState);
            }

            try
            {
                await _authService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);
                _logger.LogInformation("Password reset successfully for: {Email}", model.Email);
                return Ok(new { Message = "Password has been reset successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset for: {Email}", model.Email);
                return BadRequest(new { Error = ex.Message });
            }
        }

    }
}
