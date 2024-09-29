using jobSeeker.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.TokenService
{
    public class TokenService:ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public TokenService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            // Generates the password reset token
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> ValidatePasswordResetTokenAsync(ApplicationUser user, string token)
        {
            // Validates the reset token for the user
            return await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", token);
        }

    }
}
