using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<bool> ValidatePasswordResetTokenAsync(ApplicationUser user, string token);
    }
}
