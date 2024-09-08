using jobSeeker.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jobSeeker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthTestController : ControllerBase
    {
        [HttpGet("authenticate")]
        [Authorize]  
        public async Task<ActionResult<string>> GetSomething()
        {
            return "you are authenticated";
        }

        [HttpGet("admin-only/{id:int}")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<ActionResult<string>> GetSomething(int somevalue)
        {
            return "you are authorized with role admin";
        }
    }
}
