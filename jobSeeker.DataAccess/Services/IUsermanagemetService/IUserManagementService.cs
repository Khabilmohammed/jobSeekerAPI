﻿using jobSeeker.Models;
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
        //Task UpdateUserAsync(string userId, UpdateUserDTO updateUserDto);
        Task DeleteUserAsync(string userId);
        Task<ApplicationUser> GetUserDetailsAsync(string userId);
        Task<List<ApplicationUser>> GetUsersByRoleAsync(string role);
        Task<bool> DeactivateUserAsync(string userId);
        Task<bool> ChangeUserRoleAsync(string userId, string newRole);
    }
}
