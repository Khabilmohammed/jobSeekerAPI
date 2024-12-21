using jobSeeker.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.V2;

namespace jobSeeker.Controllers
{
    [Route("api/Payment")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration; 
        private readonly ApplicationDbContext   _context;
        public PaymentController(IConfiguration configuration,ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context; 
        }

        [HttpPost("makepayment")]
        public async Task<ActionResult> Makepayment(string userId)
        {
            var user = await _context.Users
                          .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            #region Create Payment Intent
            try
            {
                StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (int)(10 * 100),
                    Currency = "usd",
                    Description = "Purchase of digital services",
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "UserId", userId },
                        { "Email", user.Email }
                    }
                };
                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);
                return Ok(new
                {
                    ClientSecret = paymentIntent.ClientSecret,
                    Amount = options.Amount,
                    Currency = options.Currency,
                    PaymentIntentId = paymentIntent.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Payment error: {ex.Message}" });
            }
            #endregion


        }
    }
}
