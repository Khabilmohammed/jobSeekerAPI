using jobSeeker.DataAccess.Services.IJobPostingService;
using jobSeeker.DataAccess.Services.PymentService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace jobSeeker.Controllers
{
    [Route("api/JobPosting")]
    [ApiController]
    public class JobPostingController : ControllerBase
    {
        private readonly IJobPostingServices _service;
        private readonly ILogger<JobPostingController> _logger;
        public JobPostingController(IJobPostingServices service, ILogger<JobPostingController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateJobPosting([FromBody] CreateJobPostingDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating a job posting.");
                    return BadRequest(ResponseHelper.Error("Invalid input data", HttpStatusCode.BadRequest));
                }

                _logger.LogInformation("Creating a new job posting.");

                // Call the service to create a new job posting
                var createdJob = await _service.CreateJobPostingAsync(createDTO);

                // Check if the job posting was successfully created
                if (createdJob == null)
                {
                    _logger.LogWarning("Failed to create the job posting.");
                    return BadRequest(ResponseHelper.Error("Job posting creation failed", HttpStatusCode.BadRequest));
                }

                _logger.LogInformation("Job posting created successfully with ID: {JobId}", createdJob.JobId);
                return CreatedAtAction(nameof(GetJobPostingById), new { jobId = createdJob.JobId }, ResponseHelper.Success(createdJob));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new job posting.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ResponseHelper.Error("An unexpected error occurred", HttpStatusCode.InternalServerError));
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllJobPostings()
        {
            try
            {
                _logger.LogInformation("Fetching all job postings.");
                var result = await _service.GetAllJobPostingsAsync();

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No job postings found.");
                    return NotFound(ResponseHelper.Error("No job postings found", HttpStatusCode.NotFound));
                }

                return Ok(ResponseHelper.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all job postings.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ResponseHelper.Error("An unexpected error occurred", HttpStatusCode.InternalServerError));
            }
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetJobPostingById(int jobId)
        {
            try
            {
                _logger.LogInformation("Fetching job posting with ID: {JobId}", jobId);
                var result = await _service.GetJobPostingByIdAsync(jobId);

                if (result == null)
                {
                    _logger.LogWarning("Job posting not found for ID: {JobId}", jobId);
                    return NotFound(ResponseHelper.Error("Job posting not found", HttpStatusCode.NotFound));
                }

                return Ok(ResponseHelper.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching job posting by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ResponseHelper.Error("An unexpected error occurred", HttpStatusCode.InternalServerError));
            }
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetCompanyJobPostings(int companyId)
        {
            try
            {
                _logger.LogInformation("Fetching job postings for Company ID: {CompanyId}", companyId);
                var result = await _service.GetCompanyJobPostingsAsync(companyId);

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No job postings found for Company ID: {CompanyId}", companyId);
                    return NotFound(ResponseHelper.Error("No job postings found for this company", HttpStatusCode.NotFound));
                }

                return Ok(ResponseHelper.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching company job postings.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ResponseHelper.Error("An unexpected error occurred", HttpStatusCode.InternalServerError));
            }
        }

        [HttpPut("{jobId}/update")]
        public async Task<IActionResult> UpdateJobPosting(int jobId, [FromBody] UpdateJobPostingDTO updateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating job posting.");
                    return BadRequest(ResponseHelper.Error("Invalid input data", HttpStatusCode.BadRequest));
                }

                _logger.LogInformation("Updating job posting with ID: {JobId}", jobId);
                var isUpdated = await _service.UpdateJobPostingAsync(jobId, updateDTO);

                if (!isUpdated)
                {
                    _logger.LogWarning("Failed to update job posting. Job posting not found for ID: {JobId}", jobId);
                    return NotFound(ResponseHelper.Error("Job posting not found", HttpStatusCode.NotFound));
                }

                _logger.LogInformation("Job posting updated successfully with ID: {JobId}", jobId);
                return Ok(ResponseHelper.Success("Job posting updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the job posting.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ResponseHelper.Error("An unexpected error occurred", HttpStatusCode.InternalServerError));
            }
        }

        [HttpDelete("{jobId}/delete")]
        public async Task<IActionResult> DeleteJobPosting(int jobId)
        {
            try
            {
                _logger.LogInformation("Deleting job posting with ID: {JobId}", jobId);
                var isDeleted = await _service.DeleteJobPostingAsync(jobId);

                if (!isDeleted)
                {
                    _logger.LogWarning("Failed to delete job posting. Job posting not found for ID: {JobId}", jobId);
                    return NotFound(ResponseHelper.Error("Job posting not found", HttpStatusCode.NotFound));
                }

                _logger.LogInformation("Job posting deleted successfully with ID: {JobId}", jobId);
                return Ok(ResponseHelper.Success("Job posting deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the job posting.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ResponseHelper.Error("An unexpected error occurred", HttpStatusCode.InternalServerError));
            }
        }
    }
}
