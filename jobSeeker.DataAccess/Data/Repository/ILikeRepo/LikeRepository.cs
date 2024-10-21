using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.ILikeRepo
{
    public class LikeRepository: ILikeRepository
    {
        private readonly ApplicationDbContext _context;
        public LikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Like> AddLikeAsync(Like like)
        {
            await _context.Likes.AddAsync(like);
            await SaveChangesAsync();
            return like;
        }

        public async Task<bool> RemoveLikeAsync(int likeId)
        {
            var like = await _context.Likes.FindAsync(likeId);
            if (like == null)
            {
                return false;
            }

            _context.Likes.Remove(like);
            await SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<Like>> GetLikesForPostAsync(int postId)
        {
            return await _context.Likes
                .Where(l => l.PostId == postId)
                .Include(l => l.User)  
                .ToListAsync();
        }

        public async Task<Like> GetLikeByIdAsync(int likeId)
        {
            return await _context.Likes.FindAsync(likeId);
        }


        public async Task<IEnumerable<LikeDTO>> GetLikesByUserIdAsync(string userId)
        {
            return await _context.Likes
                .Where(l => l.UserId == userId)
                .Select(l => new LikeDTO
                {
                    UserId = l.UserId,
                    PostId = l.PostId
                })
                .ToListAsync();
        }
        public async Task<Like> GetLikeByUserAndPostAsync(string userId, int postId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);
        }

        public async Task<int> GetLikesCountForPostAsync(int postId)
        {
            return await _context.Likes.CountAsync(like => like.PostId == postId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
