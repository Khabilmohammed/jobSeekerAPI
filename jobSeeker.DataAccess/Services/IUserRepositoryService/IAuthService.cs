using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IUserRepositoryService
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDTO model);
        Task<APIResponse> LoginAsync(string email, string password);
        Task<bool> ValidateOtpAsync(OTPValidationRequestDTO model);
        Task LogoutAsync(string token);
    }
}
