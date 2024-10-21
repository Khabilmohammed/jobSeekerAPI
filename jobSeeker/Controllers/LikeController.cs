using AutoMapper;
using jobSeeker.DataAccess.Services.ILikeSerivce;
using jobSeeker.Models.DTO;
using jobSeeker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace jobSeeker.Controllers
{
    [Route("api/like")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeservice _likeService;
        private readonly IMapper _mapper;

        public LikeController(ILikeservice likeService, 
            IMapper mapper)
        {
            _likeService = likeService;
            _mapper = mapper;
        }

        [HttpPost("AddLike")]
        public async Task<IActionResult> AddLike([FromBody] LikeCreateDTO likeCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseHelper.Error("Invalid data."));
            }

            // Map the incoming DTO to a Like entity
            var like = _mapper.Map<Like>(likeCreateDto);

            // Call the service to add the like
            var likeDto = await _likeService.AddLikeAsync(like);

            // Handle duplicate like scenario
            if (likeDto == null)
            {
                return Conflict(ResponseHelper.Error("User already liked this post.")); // Return conflict error
            }

            // Return success response with the created LikeDTO
            return Ok(ResponseHelper.Success(likeDto)); // Return likeDto in the success response
        }





        [HttpDelete("RemoveLike")]
        public async Task<IActionResult> RemoveLike([FromBody] LikeDTO likeDTO)
        {
            if (!ModelState.IsValid || likeDTO.PostId <= 0 || string.IsNullOrEmpty(likeDTO.UserId))
            {
                return BadRequest(ResponseHelper.Error("Invalid data."));
            }

            // Call the service to remove the like
            var result = await _likeService.RemoveLikeAsync(likeDTO.PostId, likeDTO.UserId);

            if (!result)
            {
                return NotFound(ResponseHelper.Error("Like not found or couldn't be removed."));
            }

            return Ok(ResponseHelper.Success("Like removed successfully."));
        }


        // Get likes for a post
        [HttpGet("GetLikesForPost/{postId}")]
        public async Task<IActionResult> GetLikesForPost(int postId)
        {
            var likes = await _likeService.GetLikesForPostAsync(postId);
            if (likes == null)
            {
                return NotFound(ResponseHelper.Error("Likes not found for the post."));
            }

            var likeDTOs = _mapper.Map<IEnumerable<LikeDTO>>(likes);
            return Ok(ResponseHelper.Success(likeDTOs));
        }


        [HttpGet("GetLikesCountForPost/{postId}")]
        public async Task<IActionResult> GetLikesCountForPost(int postId)
        {
            var likesCount = await _likeService.GetLikesCountForPostAsync(postId);
            return Ok(ResponseHelper.Success(likesCount));
        }
    }
}
