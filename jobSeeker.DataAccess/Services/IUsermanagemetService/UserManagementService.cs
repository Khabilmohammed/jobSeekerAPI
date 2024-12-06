using jobSeeker.DataAccess.Data.Repository.IUserManagementRepo;
using jobSeeker.DataAccess.Services.CloudinaryService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Identity;
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
        private readonly CloudinaryServices _cloudinaryServices;

        public UserManagementService(IUserManagementRepository userManagementRepository,CloudinaryServices cloudinaryServices)
        {
            _userManagementRepository = userManagementRepository;
            _cloudinaryServices = cloudinaryServices;
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManagementRepository.GetUserByIdAsync(userId);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManagementRepository.GetAllUsersAsync();
        }

        public async Task<bool> UpdateUserAsync(UpdateUserDTO updateUserDto)
        {
            // Fetch the existing user
            var user = await _userManagementRepository.GetUserByIdAsync(updateUserDto.UserId);

            if (user == null)
                throw new ArgumentException("User not found.");

            // Map the DTO fields to the existing user object
            user.FirstName = updateUserDto.FirstName ?? user.FirstName;
            user.LastName = updateUserDto.LastName ?? user.LastName;
            user.City = updateUserDto.City ?? user.City;
            user.Country = updateUserDto.Country ?? user.Country;
            user.Pincode = updateUserDto.Pincode ?? user.Pincode;
            if (updateUserDto.ProfilePictureFile != null)
            {
                var profilePictureUrl = await _cloudinaryServices.UploadImageAsync(updateUserDto.ProfilePictureFile);
                user.ProfilePicture = profilePictureUrl?.SecureUrl?.ToString() ?? user.ProfilePicture;
            }

            // Save the updated user to the repository
            return await _userManagementRepository.UpdateUserAsync(user);
        }


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
