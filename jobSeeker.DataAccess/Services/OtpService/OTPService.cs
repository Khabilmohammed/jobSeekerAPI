using jobSeeker.DataAccess.Data;
using jobSeeker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.OtpService
{
    public class OTPService
    {
        private readonly ApplicationDbContext _context; // Your DbContext class

        public OTPService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateAndSaveOTPAsync(string userId)
        {
            string otp = GenerateOTP(); // You can implement a method to generate random OTP
            var userOTP = new UserOTP
            {
                UserId = userId,
                OTP = otp,
                OTPExpiryTime = DateTime.UtcNow.AddMinutes(10) // 10 minutes validity
            };

            try
            {
                _context.UserOTPs.Add(userOTP);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
                throw new Exception("An error occurred while saving OTP.", ex);
            }

            return otp;
        }

        public async Task<bool> ValidateOTPAsync(string userId, string otp)
        {
            var userOTP = await _context.UserOTPs
                .FirstOrDefaultAsync(o => o.UserId == userId && o.OTP == otp && o.IsUsed == false && o.OTPExpiryTime > DateTime.UtcNow);

            if (userOTP == null)
            {
                return false;
            }

            userOTP.IsUsed = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
                throw new Exception("An error occurred while validating OTP.", ex);
            }

            return true;
        }

        private string GenerateOTP()
        {
            // Securely generate a random 6-digit OTP
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[4];
                rng.GetBytes(byteArray);
                var random = BitConverter.ToUInt32(byteArray, 0) % 1000000;
                return random.ToString("D6");
            }
        }
    }

}
