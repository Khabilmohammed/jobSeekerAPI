using AutoMapper;
using jobSeeker.DataAccess.Data;
using jobSeeker.DataAccess.Services.ISavedPostService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace jobSeeker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SavedPostController : ControllerBase
    {
        private readonly ISavedPostservices _savedPostService;
        private readonly ILogger<SavedPostController> _logger;
        private readonly IMapper _mapper;
        public SavedPostController(ISavedPostservices savedPostService,
           ILogger<SavedPostController> logger,
            IMapper mapper)
        {
            _savedPostService = savedPostService;
            _logger = logger;
            _mapper=mapper;
        }
        [HttpPost("{postId}")]
        public async Task<IActionResult> SavePost(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(ResponseHelper.Error("User is not authenticated."));

            try
            {
                var result = await _savedPostService.SavePostAsync(userId, postId);
                if (!result) return BadRequest(ResponseHelper.Error("Post already saved or an error occurred."));

                return Ok(ResponseHelper.Success("Post saved successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving post");
                return StatusCode(500, ResponseHelper.Error("An error occurred while saving the post."));
            }
        }




        [HttpDelete("{postId}")]
        public async Task<IActionResult> RemoveSavedPost(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(ResponseHelper.Error("User is not authenticated."));

            try
            {
                var result = await _savedPostService.RemoveSavedPostAsync(userId, postId);
                if (!result) return NotFound(ResponseHelper.Error("Saved post not found."));

                return Ok(ResponseHelper.Success("Post removed from saved posts."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while removing saved post");
                return StatusCode(500, ResponseHelper.Error("An error occurred while removing the saved post."));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSavedPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("GetSavedPosts failed: User is not authenticated.");
                return Unauthorized(ResponseHelper.Error("User is not authenticated."));
            }

            try
            {
                var savedPosts = await _savedPostService.GetUserSavedPostsAsync(userId);
                if (savedPosts == null || !savedPosts.Any())
                {
                    _logger.LogWarning("No saved posts found for user: {UserId}", userId);
                    return NotFound(ResponseHelper.Error("No saved posts found."));
                }

                var savedPostDtos = _mapper.Map<IEnumerable<SavePostDTO>>(savedPosts); // Map to DTOs
                _logger.LogInformation("Saved posts retrieved successfully for user: {UserId}", userId);
                return Ok(ResponseHelper.Success(savedPostDtos)); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving saved posts for user: {UserId}", userId);
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }
    }
}
