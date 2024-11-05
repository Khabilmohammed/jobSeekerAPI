using AutoMapper;
using jobSeeker.DataAccess.Services.IExperienceService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace jobSeeker.Controllers
{
    [Route("api/Experience")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly IExperienceServices _experienceService;
        private readonly IMapper _mapper;
        private readonly ILogger<ExperienceController> _logger;

        public ExperienceController(IExperienceServices experienceService, 
            IMapper mapper, 
            ILogger<ExperienceController> logger)
        {
            _experienceService = experienceService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<List<ExperienceDto>>> GetExperiences(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("UserId was not provided.");
                    return BadRequest(ResponseHelper.Error("UserId must be provided."));
                }

                // Check if user exists
                var userExists = await _experienceService.CheckUserExistsAsync(userId);
                if (!userExists)
                {
                    _logger.LogWarning("User with Id {UserId} does not exist.", userId);
                    return NotFound(ResponseHelper.Error("User not found."));
                }

                var experiences = await _experienceService.GetAllExperiencesAsync(userId);
                return Ok(ResponseHelper.Success(experiences));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving experiences for user: {UserId}", userId);
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An error occurred while retrieving experiences."));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExperienceDto>> GetExperience(int id)
        {
            try
            {
                var experience = await _experienceService.GetExperienceByIdAsync(id);
                if (experience == null)
                {
                    _logger.LogWarning("Experience not found: {ExperienceId}", id);
                    return NotFound(ResponseHelper.Error("Experience not found."));
                }

                return Ok(ResponseHelper.Success(experience));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving experience: {ExperienceId}", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An error occurred while retrieving the experience."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ExperienceDto>> CreateExperience([FromBody] CreateExperienceDto createExperienceDto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("UserId is missing in the request.");
                    return BadRequest(ResponseHelper.Error("User ID is required."));
                }

                var isUserExists = await _experienceService.CheckUserExistsAsync(userId); 
                if (!isUserExists)
                {
                    _logger.LogWarning("User does not exist: {UserId}", userId);
                    return BadRequest(ResponseHelper.Error("User does not exist."));
                }

                var createdExperience = await _experienceService.CreateExperienceAsync(createExperienceDto, userId);
                _logger.LogInformation("Experience created successfully for UserId: {UserId}", userId);
                return CreatedAtAction(nameof(GetExperience), new { id = createdExperience.ExperienceId }, ResponseHelper.Success(createdExperience));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the experience.");
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An error occurred while creating the experience."));
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ExperienceDto>> UpdateExperience(int id, [FromBody] CreateExperienceDto createExperienceDto)
        {
            try
            {
                var updatedExperience = await _experienceService.UpdateExperienceAsync(id, createExperienceDto);
                if (updatedExperience == null)
                {
                    _logger.LogWarning("Experience not found for update: {ExperienceId}", id);
                    return NotFound(ResponseHelper.Error("Experience not found."));
                }

                _logger.LogInformation("Experience updated successfully: {ExperienceId}", id);
                return Ok(ResponseHelper.Success(updatedExperience));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the experience: {ExperienceId}", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An error occurred while updating the experience."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExperience(int id)
        {
            try
            {
                var result = await _experienceService.DeleteExperienceAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Experience not found or could not be deleted: {ExperienceId}", id);
                    return NotFound(ResponseHelper.Error("Experience not found or could not be deleted."));
                }

                _logger.LogInformation("Experience deleted successfully: {ExperienceId}", id);
                return Ok(ResponseHelper.Success("Experience deleted successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the experience: {ExperienceId}", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An error occurred while deleting the experience."));
            }
        }
    }
}
