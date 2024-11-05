using jobSeeker.DataAccess.Data.Repository.IUserManagementRepo;
using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IUsermanagemetService
{
    public class UserManagementService:IUserManagementService
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public UserManagementService(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManagementRepository.GetUserByIdAsync(userId);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManagementRepository.GetAllUsersAsync();
        }

        /*public async Task UpdateUserAsync(string userId, UpdateUserDTO updateUserDto)
        {
            var user = await _userManagementRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            // Update user properties from updateUserDto
            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            // other updates...

            await _userManagementRepository.UpdateUserAsync(user);
        }*/

        public async Task DeleteUserAsync(string userId)
        {
            await _userManagementRepository.DeleteUserAsync(userId);
        }

        public async Task<ApplicationUser> GetUserDetailsAsync(string userId)
        {
            return await _userManagementRepository.GetUserDetailsAsync(userId);
        }

        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string role)
        {
            return await _userManagementRepository.GetUsersByRoleAsync(role);
        }
        public async Task<bool> DeactivateUserAsync(string userId)
        {
            return await _userManagementRepository.DeactivateUserAsync(userId);
        }


        public async Task<bool> ChangeUserRoleAsync(string userId, string newRole)
        {
            // Validate the new role (you might have a list of allowed roles like "Admin", "User", "Company")
            var allowedRoles = new List<string> { "Admin", "User", "Company" };
            if (!allowedRoles.Contains(newRole))
            {
                throw new ArgumentException("Invalid role.");
            }

            // Call the repository method to handle role assignment
            return await _userManagementRepository.AssignRoleAsync(userId, newRole);
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            return await _userManagementRepository.UserExistsAsync(userId); 
        }
    }
}
