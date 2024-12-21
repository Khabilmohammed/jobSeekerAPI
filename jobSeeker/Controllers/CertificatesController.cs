using jobSeeker.DataAccess.Services.CertificateService;
using jobSeeker.DataAccess.Services.CloudinaryService;
using jobSeeker.Models.DTO;
using jobSeeker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace jobSeeker.Controllers
{
    [Route("api/Certificate")]
    [ApiController]
    [Authorize]
    public class CertificatesController : ControllerBase
    {
        private readonly ICertificateServices _certificateService;
        private readonly CloudinaryServices _cloudinaryServices;
        private readonly ILogger<CertificatesController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public CertificatesController(ICertificateServices certificateService, CloudinaryServices cloudinaryServices,
            ILogger<CertificatesController> logger, UserManager<ApplicationUser> userManager)
        {
            _certificateService = certificateService;
            _cloudinaryServices = cloudinaryServices;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<CertificateDto>> CreateCertificate([FromForm] CreateCertificateDto createCertificateDto, IFormFile image, string userId)
        {
            try
            {
                
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(ResponseHelper.Error("User not found."));
                }

                if (image != null)
                {
                    var uploadResult = await _cloudinaryServices.UploadImageAsync(image);
                    if (uploadResult.Error != null)
                    {
                        return BadRequest(ResponseHelper.Error("Image upload failed: " + uploadResult.Error.Message));
                    }

                    createCertificateDto.ImageUrl = uploadResult.SecureUrl.ToString();
                }

                var createdCertificate = await _certificateService.CreateCertificateAsync(createCertificateDto, userId);
                _logger.LogInformation("Certificate created successfully for UserId: {UserId}", userId);
                return CreatedAtAction(nameof(GetCertificate), new { id = createdCertificate.CertificateId }, ResponseHelper.Success(createdCertificate));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the certificate.");
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An error occurred while creating the certificate."));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CertificateDto>> GetCertificate(int id)
        {
            try
            {
                var certificate = await _certificateService.GetCertificateByIdAsync(id);
                if (certificate == null)
                {
                    return NotFound(ResponseHelper.Error("Certificate not found."));
                }
                return Ok(ResponseHelper.Success(certificate));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the certificate.");
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An error occurred while retrieving the certificate."));
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<CertificateDto>>> GetCertificatesByUser(string userId)
        {
            try
            {
                var certificates = await _certificateService.GetCertificatesByUserIdAsync(userId);
                return Ok(ResponseHelper.Success(certificates));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving certificates for the user.");
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An error occurred while retrieving certificates."));
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCertificate(int id, [FromHeader] string userId)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(ResponseHelper.Error("User not found."));
                }
                var certificate = await _certificateService.GetCertificateByIdAsync(id);
                if (certificate == null)
                {
                    return NotFound(ResponseHelper.Error("Certificate not found."));
                }
                if (certificate.UserId != userId)
                {
                    return Forbid(); 
                }
                await _certificateService.DeleteCertificateAsync(id);
                _logger.LogInformation("Certificate with ID {Id} deleted successfully by UserId: {UserId}", id, userId);

                return Ok(ResponseHelper.Success("Certificate deleted successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the certificate.");
                return StatusCode((int)HttpStatusCode.InternalServerError, ResponseHelper.Error("An error occurred while deleting the certificate."));
            }
        }
    }
}
