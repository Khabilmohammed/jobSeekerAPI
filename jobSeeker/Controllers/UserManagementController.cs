using jobSeeker.DataAccess.Services.IUsermanagemetService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using jobSeeker.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace jobSeeker.Controllers
{
    [Route("api/userManagement")]
    [ApiController]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserManagementController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(IUserManagementService userManagementService,
           ILogger<UserManagementController> logger)
        {
            _userManagementService = userManagementService;
            _logger=logger;
        }


        [HttpGet("getuserid/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userManagementService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return Ok(ResponseHelper.Error("User not found", HttpStatusCode.NotFound));
            }
            _logger.LogInformation("Retrieved user details for user: {UserId}", userId);
            return Ok(ResponseHelper.Success(user));
        }


        [HttpGet("getuserall")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagementService.GetAllUsersAsync();
            _logger.LogInformation("Retrieved all users, count: {Count}", users.Count);
            return Ok(ResponseHelper.Success(users));
        }


        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManagementService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found for deletion: {UserId}", userId);
                return Ok(ResponseHelper.Error("User not found", HttpStatusCode.NotFound));
            }

            await _userManagementService.DeleteUserAsync(userId);
            _logger.LogInformation("User deleted successfully: {UserId}", userId);
            return Ok(ResponseHelper.Success(statusCode: HttpStatusCode.NoContent));
        }


        [HttpGet("details/{userId}")]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            var userDetails = await _userManagementService.GetUserDetailsAsync(userId);
            if (userDetails == null)
            {
                _logger.LogWarning("User details not found for user: {UserId}", userId);
                return Ok(ResponseHelper.Error("User not found", HttpStatusCode.NotFound));
            } 
            _logger.LogInformation("Retrieved user details for user: {UserId}", userId);
            return Ok(ResponseHelper.Success(userDetails));
        }


        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            var users = await _userManagementService.GetUsersByRoleAsync(role);
            _logger.LogInformation("Retrieved users by role: {Role}, count: {Count}", role, users.Count);
            return Ok(ResponseHelper.Success(users));
        }

        [HttpPost("deactivate/{userId}")]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            var result = await _userManagementService.DeactivateUserAsync(userId);
            if (!result)
            {
                _logger.LogWarning("Failed to deactivate user: {UserId}", userId);
                return BadRequest("Failed to deactivate user");
            }
            _logger.LogInformation("Successfully deactivated user: {UserId}", userId);
            return Ok(ResponseHelper.Success("User deactivated successfully"));
        }

        [HttpPost("changeuserrole")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleRequestDTO request) // Assuming you have a DTO for the request
        {
            if (request == null || string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.NewRole))
            {
                return BadRequest("Invalid request data");
            }

            // Call the service to change the user role
            var result = await _userManagementService.ChangeUserRoleAsync(request.UserId, request.NewRole);

            if (!result)
            {
                _logger.LogWarning("Failed to change role for user: {UserId}", request.UserId);
                return BadRequest("Failed to change user role");
            }

            _logger.LogInformation("Successfully changed role for user: {UserId} to {NewRole}", request.UserId, request.NewRole);
            return Ok(ResponseHelper.Success("User role changed successfully"));
        }
    }
}
