using jobSeeker.DataAccess.Services.IFollowService;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jobSeeker.Controllers
{
    [Route("api/Follow")]
    [ApiController]
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
            var result = await _followService.FollowAsync(followDto);
            if (!result)
                return BadRequest("Unable to follow the user.");

            return Ok("Followed successfully.");
        }

        [HttpPost("unfollow")]
        public async Task<IActionResult> Unfollow([FromBody] FollowDTO followDto)
        {
            var result = await _followService.UnfollowAsync(followDto);
            if (!result)
                return BadRequest("Unable to unfollow the user.");

            return Ok("Unfollowed successfully.");
        }


        [HttpGet("{userId}/followers")]
        public async Task<IActionResult> GetFollowers(string userId)
        {
            var followers = await _followService.GetFollowersAsync(userId);
            if (followers == null)
                return NotFound("User not found or has no followers.");

            return Ok(followers);
        }

        [HttpGet("{userId}/following")]
        public async Task<IActionResult> GetFollowing(string userId)
        {
            var following = await _followService.GetFollowingAsync(userId);
            if (following == null)
                return NotFound("User not found or not following anyone.");

            return Ok(following);
        }

        [HttpGet("{followerId}/{followingId}/status")]
        public async Task<IActionResult> IsFollowing(string followerId, string followingId)
        {
            var isFollowing = await _followService.IsFollowingAsync(followerId, followingId);
            return Ok(new { isFollowing });
        }
    }
}
