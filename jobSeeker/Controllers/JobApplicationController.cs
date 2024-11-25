using jobSeeker.DataAccess.Services.IJobApplicationService;
using jobSeeker.Models.DTO;
using jobSeeker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace jobSeeker.Controllers
{
    [Route("api/JobApplication")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationServices _jobApplicationServices;
        private readonly ILogger<JobApplicationController> _logger;

        public JobApplicationController(
            IJobApplicationServices jobApplicationServices,
            ILogger<JobApplicationController> logger)
        {
            _jobApplicationServices = jobApplicationServices;
            _logger = logger;
        }

        [HttpPost("Apply")]
        public async Task<IActionResult> ApplyForJob([FromForm] CreateJobApplicationDTO jobApplicationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ResponseHelper.Error("Invalid data provided."));
                }

                var jobApplication = await _jobApplicationServices.ApplyForJobAsync(jobApplicationDto);
                if (jobApplication == null)
                {
                    return BadRequest(ResponseHelper.Error("Failed to apply for the job."));
                }

                return CreatedAtAction(nameof(GetApplicationById), new { jobApplicationId = jobApplication.JobApplicationId }, jobApplication);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while applying for a job.");
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An unexpected error occurred while applying for the job."));
            }
        }

        [HttpGet("{jobApplicationId}")]
        public async Task<IActionResult> GetApplicationById(int jobApplicationId)
        {
            try
            {
                var jobApplication = await _jobApplicationServices.GetApplicationByIdAsync(jobApplicationId);
                if (jobApplication == null)
                {
                    return NotFound(ResponseHelper.Error("Job application not found."));
                }
                return Ok(ResponseHelper.Success(jobApplication));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving job application with ID {jobApplicationId}.");
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An unexpected error occurred."));
            }
        }

        [HttpGet("ByJobPosting/{jobPostingId}")]
        public async Task<IActionResult> GetApplicationsForJobPosting(int jobPostingId)
        {
            try
            {
                var applications = await _jobApplicationServices.GetApplicationsForJobPostingAsync(jobPostingId);
                return Ok(ResponseHelper.Success(applications));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving applications for job posting ID {jobPostingId}.");
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An unexpected error occurred."));
            }
        }

        [HttpGet("ByUser/{userId}")]
        public async Task<IActionResult> GetUserApplications(string userId)
        {
            try
            {
                var applications = await _jobApplicationServices.GetUserApplicationsAsync(userId);
                return Ok(ResponseHelper.Success(applications));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving applications for user ID {userId}.");
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An unexpected error occurred."));
            }
        }

        [HttpPut("ChangeStatus/{jobApplicationId}")]
        public async Task<IActionResult> ChangeApplicationStatus(int jobApplicationId, [FromBody] string status)
        {
            try
            {
                var result = await _jobApplicationServices.ChangeApplicationStatusAsync(jobApplicationId, status);
                if (!result)
                {
                    return BadRequest(ResponseHelper.Error("Failed to change application status."));
                }

                return Ok(ResponseHelper.Success("Application status updated successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while changing status for job application ID {jobApplicationId}.");
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An unexpected error occurred."));
            }
        }

        [HttpDelete("{jobApplicationId}")]
        public async Task<IActionResult> RemoveApplication(int jobApplicationId)
        {
            try
            {
                var result = await _jobApplicationServices.RemoveApplicationAsync(jobApplicationId);
                if (!result)
                {
                    return BadRequest(ResponseHelper.Error("Failed to remove application."));
                }

                return Ok(ResponseHelper.Success("Application removed successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while removing job application ID {jobApplicationId}.");
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An unexpected error occurred."));
            }
        }
    }
}
