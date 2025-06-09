using jobSeeker.DataAccess.Services.IFollowService;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jobSeeker.Controllers
{
    [Route("api/Follow")]
    [ApiController]
    [Authorize]
    public class FollowController : ControllerBase
    {
        private readonly IFollowServices _followService;
        public FollowController(IFollowServices followService)
        {
            _followService = followService;
        }

        [HttpPost("follow")]
        public async Task<IActionResult> Follow([FromBody] FollowDTO followDto)
        {
            try
            {
                var result = await _followService.FollowAsync(followDto);
                if (!result)
                    return BadRequest("Unable to follow the user.");

                return Ok("Followed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while following: {ex.Message}");
            }

           
        }

        [HttpPost("unfollow")]
        public async Task<IActionResult> Unfollow([FromBody] FollowDTO followDto)
        {
            try
            {
                var result = await _followService.UnfollowAsync(followDto);
                if (!result)
                    return BadRequest("Unable to unfollow the user.");

                return Ok("Unfollowed successfully.");
            }catch (Exception ex)
            {
                return StatusCode(500, $"Error while unfollowing: {ex.Message}");
            }
           
        }


        [HttpGet("{userId}/followers")]
        public async Task<IActionResult> GetFollowers(string userId)
        {
            try
            {
                var followers = await _followService.GetFollowersAsync(userId);
                if (followers == null)
                    return NotFound("User not found or has no followers.");

                return Ok(followers);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error while unfollowing: {ex.Message}");
            }

        }

        [HttpGet("{userId}/following")]
        public async Task<IActionResult> GetFollowing(string userId)
        {
            try
            {
                var following = await _followService.GetFollowingAsync(userId);
                if (following == null)
                    return NotFound("User not found or not following anyone.");

                return Ok(following);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while retrieving following list: {ex.Message}");
            }
        }

        [HttpGet("{followerId}/{followingId}/status")]
        public async Task<IActionResult> IsFollowing(string followerId, string followingId)
        {
           try
            {
                var isFollowing = await _followService.IsFollowingAsync(followerId, followingId);
                return Ok(new { isFollowing });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while checking follow status: {ex.Message}");
            }
        }

        [HttpGet("{userId}/people-you-may-know")]
        public async Task<IActionResult> GetPeopleYouMayKnow(string userId, [FromQuery] int count = 3)
        {
            try
            {
                var suggestions = await _followService.GetPeopleYouMayKnowAsync(userId, count);
                if (suggestions == null || !suggestions.Any())
                    return NotFound("No suggestions available.");

                return Ok(suggestions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while retrieving suggestions: {ex.Message}");
            }
        }



    }
}
