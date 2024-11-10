using jobSeeker.Models.DTO;
using jobSeeker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using jobSeeker.DataAccess.Services.ICompanyService;

namespace jobSeeker.Controllers
{
    [Route("api/Company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyServices _companyService;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyServices companyService, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDTO createCompanyDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating company.");
                    return BadRequest(ResponseHelper.Error("Invalid input data", HttpStatusCode.BadRequest));
                }

                // Check if a company already exists for the user
                var existingCompany = await _companyService.GetCompanyByUserIdAsync(createCompanyDTO.UserId);
                if (existingCompany != null)
                {
                    _logger.LogWarning("Company already exists for User ID: {UserId}", createCompanyDTO.UserId);
                    return Conflict(ResponseHelper.Error("A company profile already exists for this user.", HttpStatusCode.Conflict));
                }

                var createdCompany = await _companyService.CreateCompanyAsync(createCompanyDTO);
                if (createdCompany == null)
                {
                    _logger.LogError("Failed to create company.");
                    return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.Error("Failed to create company", HttpStatusCode.InternalServerError));
                }

                _logger.LogInformation("Company successfully created with ID: {CompanyId}", createdCompany.CompanyId);
                return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompany.CompanyId }, ResponseHelper.Success(createdCompany));
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving the company by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.Error("An unexpected error occurred", HttpStatusCode.InternalServerError));
            }
            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            try
            {
                var company = await _companyService.GetCompanyByIdAsync(id);
                if (company == null)
                {
                    _logger.LogWarning("Company not found with ID: {CompanyId}", id);
                    return NotFound(ResponseHelper.Error("Company not found", HttpStatusCode.NotFound));
                }

                _logger.LogInformation("Retrieved company details for ID: {CompanyId}", id);
                return Ok(ResponseHelper.Success(company));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the company by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.Error("An unexpected error occurred", HttpStatusCode.InternalServerError));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyDTO updateCompanyDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating company with ID: {CompanyId}", id);
                    return BadRequest(ResponseHelper.Error("Invalid input data", HttpStatusCode.BadRequest));
                }

                var success = await _companyService.UpdateCompanyAsync(id, updateCompanyDTO);
                if (!success)
                {
                    _logger.LogError("Failed to update company with ID: {CompanyId}", id);
                    return NotFound(ResponseHelper.Error("Company not found or update failed", HttpStatusCode.NotFound));
                }

                _logger.LogInformation("Successfully updated company with ID: {CompanyId}", id);
                return Ok(ResponseHelper.Success("Company updated successfully", HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the company.");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.Error("An unexpected error occurred", HttpStatusCode.InternalServerError));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                var success = await _companyService.DeleteCompanyAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Failed to delete company with ID: {CompanyId}", id);
                    return NotFound(ResponseHelper.Error("Company not found", HttpStatusCode.NotFound));
                }

                _logger.LogInformation("Successfully deleted company with ID: {CompanyId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the company.");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.Error("An unexpected error occurred", HttpStatusCode.InternalServerError));
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCompanyByUserId(string userId)
        {
            try
            {
                var company = await _companyService.GetCompanyByUserIdAsync(userId);
                if (company == null)
                {
                    _logger.LogWarning("Company not found for User ID: {UserId}", userId);
                    return NotFound(ResponseHelper.Error("Company not found", HttpStatusCode.NotFound));
                }

                _logger.LogInformation("Retrieved company details for User ID: {UserId}", userId);
                return Ok(ResponseHelper.Success(company));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the company by User ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.Error("An unexpected error occurred", HttpStatusCode.InternalServerError));
            }
        }

    }
}
