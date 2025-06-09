using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IUserManagementRepo
{
    public interface IUserManagementRepository
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task DeleteUserAsync(string userId);
        Task<ApplicationUser> GetUserDetailsAsync(string userId);
        Task<List<ApplicationUser>> GetUsersByRoleAsync(string role);
        Task<bool> DeactivateUserAsync(string userId);
        Task<bool> AssignRoleAsync(string userId, string newRole);
        Task<bool> UserExistsAsync(string userId);

        Task<bool> ReactivateUserAsync(string userId);

    }
}
