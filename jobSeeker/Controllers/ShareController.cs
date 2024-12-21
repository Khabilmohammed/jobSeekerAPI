using jobSeeker.DataAccess.Data.Repository.IShareRepo;
using jobSeeker.DataAccess.Services.IShareService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace jobSeeker.Controllers
{
    [Route("api/Share")]
    [ApiController]
    public class ShareController : ControllerBase
    {
        private readonly IShareServices _shareService;
        public ShareController(IShareServices shareService)
        {
            _shareService = shareService;
        }

        [HttpPost]
        public async Task<IActionResult> SharePost([FromBody] CreateShareDTO shareDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shareService.SharePostAsync(shareDTO);
            if (result == null)
            {
                return BadRequest("Unable to share the post.");
            }

            return Ok(result);
        }

        [HttpGet("shared")]
        public async Task<IActionResult> GetSharedPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User is not authenticated.");

            try
            {
                var sharedPosts = await _shareService.GetUserSharedPostsAsync(userId);
                if (!sharedPosts.Any())
                {
                    return NotFound("No shared posts found.");
                }

                return Ok(sharedPosts); // Return shared posts directly
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving shared posts.");
            }
        }
    }
}
