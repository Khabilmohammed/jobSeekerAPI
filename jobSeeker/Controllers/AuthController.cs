
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
        private APIResponse _response;
        private readonly AuthSevice _authService;
        private readonly OTPService _otpService;
        private readonly IEmailservice _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
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
            _response = new APIResponse();
            _authService = authService; 
        }
        private static readonly Dictionary<string, RegisterRequestDTO> _temporaryStorage = new();
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            try
            {
                var existingUser = await _userRepository.GetUserByUsernameAsync(model.Username);
                if (existingUser != null)
                {
                    return BadRequest(new APIResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "User already exists with this username." }
                    });
                }

                existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return BadRequest(new APIResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "User already exists with this email." }
                    });
                }

                var otp = await _otpService.GenerateAndSaveOTPAsync(model.Email);

                var subject = "Your OTP Code";
                var body = $"Your OTP code is {otp}. It is valid for 10 minutes. Please complete the registraion.";
                await _emailService.SendEmailAsync(model.Email, subject, body);

                // Store registration details temporarily
                _temporaryStorage[model.Email] = model;

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = new List<string> { "An OTP has been sent to your email." }
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An unexpected error occurred. Please try again later." }
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var loginResponse = await _authService.LoginAsync(model.Email, model.Password);
                if (loginResponse == null)
                {
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Invalid username or password.");
                    return Unauthorized(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = loginResponse;
                return Ok(_response);
            }catch (Exception ex)
            {
                
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("An unexpected error occurred. error during login .Please try again later.");
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }


        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOTP([FromBody] OTPValidationRequestDTO model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.OTP))
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid request data." }
                });
            }

            try
            {
                var isValid = await _otpService.ValidateOTPAsync(model.Email, model.OTP);

                if (!isValid)
                {
                    return BadRequest(new APIResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Invalid or expired OTP." }
                    });
                }

                // Retrieve registration details from temporary storage
                if (!_temporaryStorage.TryGetValue(model.Email, out var registrationDetails))
                {
                    return BadRequest(new APIResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Registration details not found." }
                    });
                }

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
                {
                    return BadRequest(new APIResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = result.Errors.Select(e => e.Description).ToList()
                    });
                }

                await EnsureRolesExist();

                string role = registrationDetails.Role.ToLower();
                if (role == SD.Role_Admin)
                {
                    await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                }
                else if (role == SD.Role_Company)
                {
                    await _userManager.AddToRoleAsync(newUser, SD.Role_Company);
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, SD.Role_User);
                }

                // Remove registration details from temporary storage after successful registration
                _temporaryStorage.Remove(model.Email);

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = "User registered successfully and email confirmed."
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An unexpected error occurred. Please try again later." }
                });
            }
        }
        private async Task EnsureRolesExist()
        {
            if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
            }
            if (!await _roleManager.RoleExistsAsync(SD.Role_User))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_User));
            }
            if (!await _roleManager.RoleExistsAsync(SD.Role_Company))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Company));
            }
        }
    }
}
