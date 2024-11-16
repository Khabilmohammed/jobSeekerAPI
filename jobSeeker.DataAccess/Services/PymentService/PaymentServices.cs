using jobSeeker.DataAccess.Data;
using jobSeeker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.PymentService
{
    public class PaymentServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _stripeSecretKey;

        public PaymentServices(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _stripeSecretKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<Payment> CreatePaymentIntentAsync(decimal amount, string stripeToken, string userId, string transactionDescription)
        {
            StripeConfiguration.ApiKey = _stripeSecretKey;

            var paymentMethodOptions = new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardOptions { Token = stripeToken }
            };

            var paymentMethodService = new PaymentMethodService();
            var paymentMethod = await paymentMethodService.CreateAsync(paymentMethodOptions);

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = "usd",
                PaymentMethod = paymentMethod.Id,
                Description = transactionDescription,
                Confirm = false,
            };

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.CreateAsync(options);

            // Save payment intent details in the database
            var payment = new Payment
            {
                Amount = amount,
                Currency = "USD",
                PaymentStatus = paymentIntent.Status,
                PaymentIntentId = paymentIntent.Id, // Store the PaymentIntent ID (starts with `pi_`)
                ClientSecret = paymentIntent.ClientSecret,
                JobId = null,
                PaymentDate = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }


        public async Task<bool> ConfirmPaymentAsync(string paymentIntentId)
        {
            StripeConfiguration.ApiKey = _stripeSecretKey;

            var service = new PaymentIntentService();
            var paymentIntent = await service.ConfirmAsync(paymentIntentId);

            // Update payment status
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId);
            if (payment == null) return false;

            payment.PaymentStatus = paymentIntent.Status;
            await _context.SaveChangesAsync();

            return paymentIntent.Status == "succeeded";
        }
    }
}
