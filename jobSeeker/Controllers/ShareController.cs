using jobSeeker.DataAccess.Data.Repository.IShareRepo;
using jobSeeker.DataAccess.Services.IShareService;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jobSeeker.Controllers
{
    [Route("api/Share")]
    [ApiController]
    public class ShareController : ControllerBase
    {
        private readonly IShareServices _shareService;
        public ShareController(IShareServices shareService)
        {
            _shareService = shareService;
        }

        [HttpPost]
        public async Task<IActionResult> SharePost([FromBody] CreateShareDTO shareDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shareService.SharePostAsync(shareDTO);
            if (result == null)
            {
                return BadRequest("Unable to share the post.");
            }

            return Ok(result);
        }

        [HttpGet("UserShares/{userId}")]
        public async Task<IActionResult> GetUserShares(string userId)
        {
            var shares = await _shareService.GetSharesByUserIdAsync(userId);

            if (shares == null || !shares.Any())
                return NotFound("No shares found for this user.");

            return Ok(shares);
        }

        // GET: api/Share/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShareById(int id)
        {
            var share = await _shareService.GetShareByIdAsync(id);

            if (share == null)
                return NotFound($"Share with ID {id} not found.");

            return Ok(share);
        }
    }
}
