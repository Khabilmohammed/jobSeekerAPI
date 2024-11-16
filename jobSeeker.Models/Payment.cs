using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }  // Primary key
        public decimal Amount { get; set; } // The amount paid
        public string Currency { get; set; } // Currency (e.g., USD, INR)
        public string PaymentStatus { get; set; } // Status (e.g., "Succeeded", "Failed")
        public string PaymentIntentId { get; set; } // Stripe Payment Intent ID
        public string PaymentMethodId { get; set; } // Stripe Payment Method ID
        public string ClientSecret { get; set; } // Stripe Client Secret used for confirming payments
        public DateTime PaymentDate { get; set; } // Date and time of payment
        [ForeignKey("JobPosting")]
        public int? JobId { get; set; }
        public JobPosting JobPosting { get; set; }
    }
}
