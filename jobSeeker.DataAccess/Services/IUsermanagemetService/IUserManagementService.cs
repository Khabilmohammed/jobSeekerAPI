using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IUsermanagemetService
{
    public interface IUserManagementService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task DeleteUserAsync(string userId);
        Task<ApplicationUser> GetUserDetailsAsync(string userId);
        Task<List<ApplicationUser>> GetUsersByRoleAsync(string role);
        Task<bool> DeactivateUserAsync(string userId);
        Task<bool> ChangeUserRoleAsync(string userId, string newRole);
        Task<bool> UserExistsAsync(string userId);
        Task<bool> UpdateUserAsync(UpdateUserDTO updateUserDto);
/*        Task<IEnumerable<UserDTOSerch>> SearchUsersAsync(string query);
*/
    }
}
