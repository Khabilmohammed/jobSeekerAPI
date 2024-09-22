using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class UserOTP
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public string OTP { get; set; }
        public DateTime OTPExpiryTime { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
