using jobSeeker.DataAccess.Services.CloudinaryService;
using jobSeeker.DataAccess.Services.IStoryService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace jobSeeker.Controllers
{
    [Route("api/stories")]
    [ApiController]
    public class StoryController : Controller
    {
        private readonly IStoryServices _storyService;
        private readonly ILogger<StoryController> _logger;
        private readonly CloudinaryServices _cloudinaryService;

        public StoryController(IStoryServices storyService, CloudinaryServices cloudinaryService, ILogger<StoryController> logger)
        {
            _storyService = storyService;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }

        [HttpGet("getallstories")]
        public async Task<IActionResult> GetAllStories()
        {
            var stories = await _storyService.GetAllStoriesAsync(); 
            int count = stories.Count();
            _logger.LogInformation("Retrieved all stories, count: {Count}", count);
            return Ok(ResponseHelper.Success(stories)); 
        }

        [HttpGet("{storyId}")]
        public async Task<IActionResult> GetStoryById(int storyId) 
        {
            var story = await _storyService.GetStoryByIdAsync(storyId);
            if (story == null)
            {
                _logger.LogWarning("Story not found: {StoryId}", storyId);
                return NotFound(ResponseHelper.Error("Story not found", HttpStatusCode.NotFound));
            }

            _logger.LogInformation("Retrieved story details for story: {StoryId}", storyId);
            return Ok(ResponseHelper.Success(story));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetStoriesByUserId(string userId)
        {
            var stories = await _storyService.GetStoriesByUserIdAsync(userId);
            int count = stories.Count();
            _logger.LogInformation("Retrieved stories for user: {UserId}, count: {Count}", userId, count);
            return Ok(ResponseHelper.Success(stories));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateStory([FromForm] CreateStoryDTO storyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var uploadResult = await _cloudinaryService.UploadImageAsync(storyDto.Image);
            if (uploadResult.Error != null)
            {
                _logger.LogError("Image upload to Cloudinary failed: {Error}", uploadResult.Error.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Image upload failed");
            }

            storyDto.ImageUrl = uploadResult.SecureUrl.ToString(); 

            var createdStoryDto = await _storyService.AddStoryAsync(storyDto); 
            _logger.LogInformation("Successfully created a story for user: {UserId}", storyDto.UserId);

            return Ok(ResponseHelper.Success(createdStoryDto)); 
        }

        [HttpDelete("{storyId}")]
        public async Task<IActionResult> DeleteStory(int storyId) 
        {
            var story = await _storyService.GetStoryByIdAsync(storyId);
            if (story == null)
            {
                _logger.LogWarning("Story not found for deletion: {StoryId}", storyId);
                return NotFound(ResponseHelper.Error("Story not found", HttpStatusCode.NotFound));
            }

            var isDeleted = await _storyService.RemoveStoryAsync(storyId);
            if (!isDeleted) 
            {
                _logger.LogError("Failed to delete story: {StoryId}", storyId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Story deletion failed");
            }

            _logger.LogInformation("Story deleted successfully: {StoryId}", storyId);
            return NoContent(); 
        }

       /* [HttpPost("mark-inactive")]
        public async Task<IActionResult> MarkStoriesAsInactive()
        {
            await _storyService.MarkStoriesAsInactiveAsync();
            _logger.LogInformation("Stories marked as inactive successfully.");
            return Ok(ResponseHelper.Success("Stories marked as inactive"));
        }*/
    }
}
