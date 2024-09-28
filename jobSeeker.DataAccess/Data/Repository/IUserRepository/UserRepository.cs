using jobSeeker.DataAccess.Data;
using jobSeeker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IUserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task AddUserAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email) // New method implementation
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            return await _db.Users.FindAsync(userId);
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be null or empty", nameof(username));

            return await _db.Users.SingleOrDefaultAsync(u => u.UserName == username);
        }
    }
}
