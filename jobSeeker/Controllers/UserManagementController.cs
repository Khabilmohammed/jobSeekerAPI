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
        [Authorize]
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
        [Authorize]
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

        [HttpPost("reactivate/{userId}")]
        public async Task<IActionResult> ReactivateUser(string userId)
        {
            var result = await _userManagementService.ReactivateUserAsync(userId);
            if (!result)
            {
                _logger.LogWarning("Failed to reactivate user: {UserId}", userId);
                return BadRequest("Failed to reactivate user");
            }

            _logger.LogInformation("Successfully reactivated user: {UserId}", userId);
            return Ok(ResponseHelper.Success("User reactivated successfully"));
        }

        [HttpPost("changeuserrole")]
        [Authorize]
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

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserId))
            {
                _logger.LogWarning("Invalid update request received");
                return BadRequest("Invalid request data.");
            }

            var user = await _userManagementService.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found for update: {UserId}", request.UserId);
                return Ok(ResponseHelper.Error("User not found", HttpStatusCode.NotFound));
            }

            var result = await _userManagementService.UpdateUserAsync(request);

            if (!result)
            {
                _logger.LogWarning("Failed to update user: {UserId}", request.UserId);
                return BadRequest("Failed to update user.");
            }

            _logger.LogInformation("User updated successfully: {UserId}", request.UserId);
            return Ok(ResponseHelper.Success("User updated successfully."));
        }

    }
}
