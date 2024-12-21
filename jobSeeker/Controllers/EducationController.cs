using jobSeeker.DataAccess.Services.IEducationService;
using jobSeeker.Models.DTO;
using jobSeeker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using jobSeeker.DataAccess.Services.IUsermanagemetService;
using Microsoft.AspNetCore.Authorization;

namespace jobSeeker.Controllers
{
    [Route("api/Education")]
    [ApiController]
    [Authorize]
    public class EducationController : ControllerBase
    {
        private readonly IEducationServices _educationService;
        private readonly ILogger<EducationController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserManagementService _userManagementService; 

        public EducationController(IEducationServices educationService, ILogger<EducationController> logger, IMapper mapper, IUserManagementService userManagementService)
        {
            _educationService = educationService;
            _logger = logger;
            _mapper = mapper;
            _userManagementService = userManagementService;

        }
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<EducationResponseDTO>>> GetEducationsByUserId(string userId)
       {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("UserId was not provided.");
                    return BadRequest(ResponseHelper.Error("UserId must be provided."));
                }

                var educations = await _educationService.GetEducationsByUserIdAsync(userId);
                return Ok(ResponseHelper.Success(educations));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching educations for user ID: {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.Error("An error occurred while fetching educations."));
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EducationResponseDTO>> GetEducationById(int id)
        {
            try
            {
                var educationResponse = await _educationService.GetEducationByIdAsync(id);
                if (educationResponse == null)
                {
                    return NotFound(ResponseHelper.Error("Education not found."));
                }
                return Ok(ResponseHelper.Success(educationResponse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching education with ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.Error("An error occurred while fetching the education."));
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<EducationResponseDTO>> CreateEducation([FromBody] CreateEducationDTO createEducationDto, [FromHeader(Name = "UserId")] string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest(ResponseHelper.Error("Invalid or missing user ID."));
                }

                // Check if the user exists
                if (!await _userManagementService.UserExistsAsync(userId))
                {
                    return NotFound(ResponseHelper.Error("User not found.")); // or return BadRequest based on your design
                }

                createEducationDto.UserId = userId;

                var education = await _educationService.AddEducationAsync(createEducationDto);
                var educationResponse = _mapper.Map<EducationResponseDTO>(education);

                return CreatedAtAction(nameof(GetEducationById), new { id = education.EducationId }, ResponseHelper.Success(educationResponse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating education.");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.Error("An error occurred while creating the education."));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EducationResponseDTO>> UpdateEducation(int id, [FromBody] EducationResponseDTO createEducationDto)
        {
            try
            {
                var updatedEducation = await _educationService.UpdateEducationAsync(id, createEducationDto);
                if (updatedEducation == null)
                {
                    return NotFound(ResponseHelper.Error("Education not found."));
                }
                return Ok(ResponseHelper.Success(updatedEducation));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating education with ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.Error("An error occurred while updating the education."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEducation(int id)
        {
            try
            {
                var result = await _educationService.DeleteEducationAsync(id);
                if (!result)
                {
                    return NotFound(ResponseHelper.Error("Education not found."));
                }
                return Ok(ResponseHelper.Success("Education deleted successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting education with ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.Error("An error occurred while deleting the education."));
            }
        }
    }
}
