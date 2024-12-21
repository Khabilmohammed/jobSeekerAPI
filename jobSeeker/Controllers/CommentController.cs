using AutoMapper;
using jobSeeker.DataAccess.Services.ICommentService;
using jobSeeker.Models.DTO;
using jobSeeker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace jobSeeker.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentservice _commentService;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentservice commentService, IMapper mapper, ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment([FromBody] CommentDTO commentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid data provided for comment creation.");
                    return BadRequest(ResponseHelper.Error("Invalid data provided."));
                }

                var comment = _mapper.Map<Comment>(commentDTO);
                var createdComment = await _commentService.CreateCommentAsync(comment);

                // Map to CommentResponseDTO, which includes the generated CommentId
                var result = _mapper.Map<CommentResponseDTO>(createdComment);
                _logger.LogInformation("Comment created successfully for PostId: {PostId} by UserId: {UserId}", commentDTO.PostId, commentDTO.UserId);

                // Return the generated CommentId in the response
                return CreatedAtAction(nameof(GetCommentById), new { commentId = result.CommentId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the comment.");
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseHelper.Error("An error occurred while creating the comment: " + ex.Message));
            }
        }


        [HttpGet("GetComment/{commentId}")]
        public async Task<IActionResult> GetCommentById(int commentId)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(commentId);
                if (comment == null)
                {
                    _logger.LogWarning("Comment not found: {CommentId}", commentId);
                    return NotFound(ResponseHelper.Error("Comment not found."));
                }

                var commentDto = _mapper.Map<CommentDTO>(comment);
                _logger.LogInformation("Retrieved comment details for CommentId: {CommentId}", commentId);
                return Ok(ResponseHelper.Success(commentDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the comment.");
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseHelper.Error("An error occurred while retrieving the comment: " + ex.Message));
            }
        }

        [HttpDelete("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                var deleted = await _commentService.DeleteCommentAsync(commentId);
                if (!deleted)
                {
                    _logger.LogWarning("Comment not found or could not be deleted: {CommentId}", commentId);
                    return NotFound(ResponseHelper.Error("Comment not found or could not be deleted."));
                }

                _logger.LogInformation("Comment deleted successfully: {CommentId}", commentId);
                return Ok(ResponseHelper.Success("Comment deleted successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the comment.");
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseHelper.Error("An error occurred while deleting the comment: " + ex.Message));
            }
        }

        [HttpGet("GetCommentsByPost/{postId}")]
        public async Task<IActionResult> GetCommentsByPost(int postId)
        {
            try
            {
                var comments = await _commentService.GetCommentsByPostIdAsync(postId);
                var commentDtos = _mapper.Map<List<CommentResponseDTO>>(comments);

                var response = new
                {
                    Comments = commentDtos,
                    Count = commentDtos.Count
                };

                _logger.LogInformation("Retrieved {Count} comments for PostId: {PostId}", commentDtos.Count, postId);
                return Ok(ResponseHelper.Success(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving comments for PostId: {PostId}", postId);
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An error occurred while retrieving comments: " + ex.Message));
            }
        }
    }
}
