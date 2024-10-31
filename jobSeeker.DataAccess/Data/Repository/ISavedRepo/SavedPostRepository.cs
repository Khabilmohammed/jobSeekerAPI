using jobSeeker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.ISavedRepo
{
    public class SavedPostRepository : ISavedPostRepository
    {
        private readonly ApplicationDbContext _context;

        public SavedPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SavedPostExistsAsync(string userId, int postId)
        {
            return await _context.SavedPosts
                .AnyAsync(sp => sp.UserId == userId && sp.PostId == postId);
        }
        public async Task<IEnumerable<SavedPost>> GetUserSavedPostsAsync(string userId)
        {
            return await _context.SavedPosts
                .Include(sp => sp.Post) // Include Post details if needed
                .Include(sp => sp.User) // Include User details to fetch username and other user info
                .Where(sp => sp.UserId == userId)
                .ToListAsync();
        }


        public async Task AddSavedPostAsync(SavedPost savedPost)
        {
            await _context.SavedPosts.AddAsync(savedPost);
        }

        public async Task RemoveSavedPostAsync(SavedPost savedPost)
        {
            _context.SavedPosts.Remove(savedPost);
        }

        public async Task<SavedPost> GetSavedPostAsync(string userId, int postId)
        {
            return await _context.SavedPosts
                .FirstOrDefaultAsync(sp => sp.UserId == userId && sp.PostId == postId);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }


}
