
using jobSeeker.DataAccess.Services.AuthService;
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
        private readonly IUserRepository _userRepository;
        private readonly AuthSevice _authService;
        private readonly OTPService _otpService;
        private readonly IEmailservice _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private static readonly Dictionary<string, RegisterRequestDTO> _temporaryStorage = new();

        public AuthController(IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AuthSevice authService,
            IEmailservice emailService,
            OTPService otpService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _otpService = otpService;
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseHelper.Error(errors));
            }

            try
            {
                var existingUser = await _userRepository.GetUserByUsernameAsync(model.Username);
                if (existingUser != null)
                    return BadRequest(ResponseHelper.Error("User already exists with this username."));

                existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
                if (existingUser != null)
                    return BadRequest(ResponseHelper.Error("User already exists with this email."));

                var otp = await _otpService.GenerateAndSaveOTPAsync(model.Email);
                var subject = "Your OTP Code";
                var body = $"Your OTP code is {otp}. It is valid for 10 minutes. Please complete the registration.";
                await _emailService.SendEmailAsync(model.Email, subject, body);

                // Store registration details temporarily
                _temporaryStorage[model.Email] = model;

                return Ok(ResponseHelper.Success("An OTP has been sent to your email."));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    ResponseHelper.Error("An unexpected error occurred. Please try again later.", HttpStatusCode.InternalServerError));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Error("Invalid input data."));

            try
            {
                var loginResponse = await _authService.LoginAsync(model.Email, model.Password);
                if (loginResponse == null)
                    return Unauthorized(ResponseHelper.Error("Invalid username or password.", HttpStatusCode.Unauthorized));

                return Ok(ResponseHelper.Success(loginResponse));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An unexpected error occurred during login. Please try again later.", HttpStatusCode.InternalServerError));
            }
        }

        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOTP([FromBody] OTPValidationRequestDTO model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.OTP))
                return BadRequest(ResponseHelper.Error("Invalid request data."));

            try
            {
                var isValid = await _otpService.ValidateOTPAsync(model.Email, model.OTP);
                if (!isValid)
                    return BadRequest(ResponseHelper.Error("Invalid or expired OTP."));

                if (!_temporaryStorage.TryGetValue(model.Email, out var registrationDetails))
                    return BadRequest(ResponseHelper.Error("Registration details not found."));

                var newUser = new ApplicationUser
                {
                    UserName = registrationDetails.Username,
                    Email = model.Email,
                    FirstName = registrationDetails.FirstName,
                    MiddleName = registrationDetails.MiddleName,
                    LastName = registrationDetails.LastName,
                    City = registrationDetails.City,
                    Country = registrationDetails.Country,
                    Pincode = registrationDetails.Pincode,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser, registrationDetails.Password);
                if (!result.Succeeded)
                    return BadRequest(ResponseHelper.Error(result.Errors.Select(e => e.Description).ToList()));

                await EnsureRolesExist();
                await _userManager.AddToRoleAsync(newUser, registrationDetails.Role);

                _temporaryStorage.Remove(model.Email);
                return Ok(ResponseHelper.Success("User registered successfully and email confirmed."));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An unexpected error occurred. Please try again later.", HttpStatusCode.InternalServerError));
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                    return BadRequest(ResponseHelper.Error("Token is missing from the request."));

                await _authService.LogoutAsync(token);
                return Ok(ResponseHelper.Success("User logged out successfully."));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseHelper.Error("An unexpected error occurred during logout. Please try again later.", HttpStatusCode.InternalServerError));
            }
        }

        private async Task EnsureRolesExist()
        {
            if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
            if (!await _roleManager.RoleExistsAsync(SD.Role_User))
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_User));
            if (!await _roleManager.RoleExistsAsync(SD.Role_Company))
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Company));
        }
    }
}
