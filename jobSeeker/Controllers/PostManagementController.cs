﻿using AutoMapper;
using jobSeeker.DataAccess.Services.IPostService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace jobSeeker.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostManagementController : ControllerBase
    {
        private readonly IPostServices _postServices;
        private readonly IMapper _mapper;
       

        public PostManagementController(IPostServices postServices
            ,IMapper mapper)
        {
            _mapper = mapper;
            _postServices = postServices;   
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

            var postDto = _mapper.Map<PostDTO>(post);
            return Ok(ResponseHelper.Success(postDto));
        }

        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postServices.GetAllPostsAsync();
            var postDtos = _mapper.Map<IEnumerable<PostDTO>>(posts);
            return Ok(ResponseHelper.Success(postDtos));
        }


        [HttpPut("UpdatePost/{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] PostDTO postDto)
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
            if (updated!=null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseHelper.Error("Error updating the post."));
            }

            return Ok(ResponseHelper.Success("Post updated successfully."));
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