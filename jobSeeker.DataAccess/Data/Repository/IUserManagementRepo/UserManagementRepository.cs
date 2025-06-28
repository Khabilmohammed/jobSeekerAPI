using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IUserManagementRepo
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserManagementRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {

            _context = context;
            _userManager = userManager;
        }

        public async Task DeleteUserAsync(string userId)
        {
            
            var stories = _context.Stories.Where(s => s.UserId == userId).ToList();
            _context.Stories.RemoveRange(stories);
            await _context.SaveChangesAsync();
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            foreach (var user in users)
            {
                user.Role = await (from ur in _context.UserRoles
                                   join r in _context.Roles on ur.RoleId equals r.Id
                                   where ur.UserId == user.Id
                                   select r.Name).FirstOrDefaultAsync();
            }

            return users;
        }

        public async Task<ApplicationUser> GetApplicationUserByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }


        public async Task<UserProfileDTO> GetUserByIdAsync(string userId)
        {
            var user = await _context.Users
            .Include(u => u.Followers)
            .Include(u => u.Following)
            .Include(u => u.Posts)
            .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return null;

            return new UserProfileDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                City = user.City,
                Country = user.Country,
                Pincode = user.Pincode,
                ProfilePicture = user.ProfilePicture,
                UserName = user.UserName,
                FollowersCount = user.Followers?.Count ?? 0,
                FollowingCount = user.Following?.Count ?? 0,
                PostCount = user.Posts?.Count ?? 0
            };
        }

        public async Task<ApplicationUser> GetUserDetailsAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                
                if (user != null)
                {
                    // Since a user can only have one role, select the single role instead of a list
                    user.Role = await (from ur in _context.UserRoles
                                       join r in _context.Roles on ur.RoleId equals r.Id
                                       where ur.UserId == userId
                                       select r.Name).FirstOrDefaultAsync();  
                }
            }
            return user;
        }


        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string role)
        {
            return await (from u in _context.Users
                          join ur in _context.UserRoles on u.Id equals ur.UserId
                          join r in _context.Roles on ur.RoleId equals r.Id
                          where r.Name == role
                          select u).ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                _context.Users.Update(user); // Track the updated user entity
                await _context.SaveChangesAsync(); // Save changes to the database
                return true; // Update successful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeactivateUserAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Enable lockout
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.UtcNow.AddDays(3);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true; // Deactivation successful
        }

        public async Task<bool> ReactivateUserAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.LockoutEnd = null; // Remove lockout
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> AssignRoleAsync(string userId, string newRole)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Fetch the current roles of the user
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove all existing roles
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Add the new role
            var result = await _userManager.AddToRoleAsync(user, newRole);

            return result.Succeeded;
        }
        public async Task<bool> UserExistsAsync(string userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId); // Check if any user matches the ID
        }


    }
}
