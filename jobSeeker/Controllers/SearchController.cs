using jobSeeker.DataAccess.Services.IJobApplicationService;
using jobSeeker.DataAccess.Services.IJobPostingService;
using jobSeeker.DataAccess.Services.IUsermanagemetService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jobSeeker.Controllers
{
    [Route("api/Search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IUserManagementService _userService;
        private readonly IJobPostingServices _postService;

        public SearchController(IUserManagementService userService,
            IJobPostingServices postService)
        {
          _userService = userService;
            _postService = postService;
        }

       /* [HttpGet("users")]
        public async Task<IActionResult> SearchUsers([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { Message = "Search query cannot be empty." });
            }

            try
            {
                var users = await _userService.SearchUsersAsync(query);

                if (users == null || !users.Any())
                {
                    return NotFound(new { Message = "No users found for the provided query." });
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is implemented)
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
*/
        [HttpGet("job-posts")]
        public async Task<IActionResult> SearchJobPosts([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { Message = "Search query cannot be empty." });
            }

            try
            {
                var posts = await _postService.SearchJobPostsAsync(query);

                if (posts == null || !posts.Any())
                {
                    return NotFound(new { Message = "No job posts found for the provided query." });
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is implemented)
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

    }
}
