using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IUserRepository
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task AddUserAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByEmailAsync(string email);

        Task<UserOTP> GetUserByOTPtableAsync(string email);

        Task UpdatePasswordAsync(ApplicationUser user,string newPassword);

    }
}
