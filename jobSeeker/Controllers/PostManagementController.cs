using AutoMapper;
using jobSeeker.DataAccess.Services.ICommentService;
using jobSeeker.DataAccess.Services.ILikeSerivce;
using jobSeeker.DataAccess.Services.IPostService;
using jobSeeker.DataAccess.Services.IUsermanagemetService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace jobSeeker.Controllers
{
    
    [Route("api/post")]
    [ApiController]
    [Authorize]
    public class PostManagementController : ControllerBase
    {
        private readonly IPostServices _postServices;
        private readonly IMapper _mapper;
        private readonly ILikeservice _likeservice;
        private readonly ICommentservice _commentservice;
        private readonly UserManager<ApplicationUser> _userManager;


        public PostManagementController(IPostServices postServices,
            ILikeservice likeservice
            ,IMapper mapper
            ,ICommentservice commentservice
            , UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _postServices = postServices; 
            _likeservice = likeservice;
            _commentservice = commentservice;
            _userManager = userManager;
        }

        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDTO createPostDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ResponseHelper.Error("Invalid data provided."));
                }

                if (createPostDTO.Images == null || !createPostDTO.Images.Any())
                {
                    return BadRequest(new { message = "Post must include at least one image." });
                }

                var post = _mapper.Map<Post>(createPostDTO);
                var createdPost = await _postServices.CreatePostAsync(post);

                

                if (createPostDTO.Images != null && createPostDTO.Images.Any())
                {
                    foreach (var imageFile in createPostDTO.Images)
                    {
                        await _postServices.AddImageAsync(imageFile, createdPost.PostId);
                    }
                }

                var postDTO = _mapper.Map<PostDTO>(createdPost);
                return CreatedAtAction(nameof(GetPostById), new { postId = postDTO.PostId }, postDTO);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseHelper.Error("An unexpected error occurred while creating the post: " + ex.Message));
            }
        }

        [HttpGet("GetPost/{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            var post = await _postServices.GetPostByIdAsync(postId);
            if (post == null)
            {
                return NotFound(ResponseHelper.Error("Post not found."));
            }

            var postDtos = _mapper.Map<PostDTO>(post);
            postDtos.likeCount = await _likeservice.GetLikesCountForPostAsync(postId); // Add this line to get accurate like count
            return Ok(ResponseHelper.Success(postDtos));
        }

        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postServices.GetAllPostsAsync();
            var postDtos = _mapper.Map<IEnumerable<PostDTO>>(posts);
            foreach (var postDto in postDtos)
            {
                postDto.likeCount = await _likeservice.GetLikesCountForPostAsync(postDto.PostId);
                postDto.commentCount = await _commentservice.GetCommentCountForPostAsync(postDto.PostId);

                if (!string.IsNullOrEmpty(postDto.UserId))
                {
                    // Fetch the user by UserId
                    var user = await _userManager.FindByIdAsync(postDto.UserId);
                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        postDto.UserRole = roles.FirstOrDefault();
                    }
                }
            }
            return Ok(ResponseHelper.Success(postDtos));
        }


        [HttpPut("UpdatePost/{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] UpdatePostDTO postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseHelper.Error("Invalid data."));
            }

            var postToUpdate = await _postServices.GetPostByIdAsync(postId);
            if (postToUpdate == null)
            {
                return NotFound(ResponseHelper.Error("Post not found."));
            }

            _mapper.Map(postDto, postToUpdate);

            var updated = await _postServices.UpdatePostAsync(postToUpdate);
            if (updated==null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseHelper.Error("Error updating the post."));
            }

            return Ok(ResponseHelper.Success("Post updated successfully."));
        }


        [HttpGet("GetPostsByUser/{userId}")]
        public async Task<IActionResult> GetPostsByUserId(string userId)
        {
            try
            {
                var posts = await _postServices.GetPostsByUserIdAsync(userId);
                if (posts == null || !posts.Any())
                {
                    return NotFound(ResponseHelper.Error("No posts found for this user."));
                }

                var postDtos = _mapper.Map<IEnumerable<PostDTO>>(posts);

                // Optionally, calculate like count for each post
                foreach (var postDto in postDtos)
                {
                    postDto.likeCount = await _likeservice.GetLikesCountForPostAsync(postDto.PostId);
                    postDto.commentCount = await _commentservice.GetCommentCountForPostAsync(postDto.PostId); // Fetch comment count
                }

                return Ok(ResponseHelper.Success(postDtos));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseHelper.Error("An unexpected error occurred: " + ex.Message));
            }
        }



        [HttpDelete("DeletePost/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var deleted = await _postServices.DeletePostAsync(postId);
            if (!deleted)
            {
                return NotFound(ResponseHelper.Error("Post not found or could not be deleted."));
            }

            return Ok(ResponseHelper.Success("Post deleted successfully."));
        }

        [HttpGet("TestRoute")]
        public IActionResult TestRoute()
        {
            return Ok("Controller is working");
        }
    }


}
