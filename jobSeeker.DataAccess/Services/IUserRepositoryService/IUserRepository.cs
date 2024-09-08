using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IUserRepositoryService
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task AddUserAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
    }
}
