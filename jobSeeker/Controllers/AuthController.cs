
using jobSeeker.DataAccess.Services.IUserRepositoryService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using jobSeeker.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace jobSeeker.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private APIResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthController(IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
             RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager; 
            _roleManager = roleManager;
            _response = new APIResponse();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var existingUser = await _userRepository.GetUserByUsernameAsync(model.Username);
                if (existingUser != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User already exists.");
                    return BadRequest(_response);
                }
                var newUser = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    NormalizedEmail = model.Email.ToUpper(),
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    City = model.City,
                    Country = model.Country,
                    Pincode = model.Pincode,

                };
                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (!result.Succeeded)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    foreach (var error in result.Errors)
                    {
                        _response.ErrorMessages.Add(error.Description);
                    }
                    return BadRequest(_response);
                }
                await EnsureRolesExist();
                string role = model.Role.ToLower();
                if (model.Role.ToLower() == SD.Role_Admin)
                {
                    await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                }
                else if (model.Role.ToLower() == SD.Role_Company)
                {
                    await _userManager.AddToRoleAsync(newUser, SD.Role_Company);
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, SD.Role_User);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
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
