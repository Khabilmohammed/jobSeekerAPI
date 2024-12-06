using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IFollowRepo
{
    public class FollowRepository : IFollowRepository
    {
        private readonly ApplicationDbContext _context;
        public FollowRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddFollowAsync(string followerId, string followingId)
        {
            if (await _context.Follows.AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId))
                return false;

            var follow = new Follow
            {
                FollowerId = followerId,
                FollowingId = followingId
            };

            _context.Follows.Add(follow);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> RemoveFollowAsync(string followerId, string followingId)
        {
            var follow = await _context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

            if (follow == null)
                return false;

            _context.Follows.Remove(follow);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<followUserdetailDTO>> GetFollowersAsync(string userId)
        {
            return await (from follow in _context.Follows
                          join user in _context.ApplicationUsers
                          on follow.FollowerId equals user.Id
                          where follow.FollowingId == userId
                          select new followUserdetailDTO
                          {
                              UserId = user.Id,
                              FirstName = user.FirstName,
                          }).ToListAsync();
        }

        public async Task<List<followUserdetailDTO>> GetFollowingAsync(string userId)
        {
            return await (from follow in _context.Follows
                          join user in _context.ApplicationUsers
                          on follow.FollowingId equals user.Id
                          where follow.FollowerId == userId
                          select new followUserdetailDTO
                          {
                              UserId = user.Id,
                              FirstName = user.FirstName,
                          }).ToListAsync();
        }
        public async Task<bool> IsFollowingAsync(string followerId, string followingId)
        {
            return await _context.Follows
                .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
        }

        public async Task<List<followUserdetailDTO>> GetPeopleYouMayKnowAsync(string userId, int count)
        {
            var followedUsers = _context.Follows
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FollowingId);

            return await _context.ApplicationUsers
                .Where(user => user.Id != userId && !followedUsers.Contains(user.Id))
                .Select(user => new followUserdetailDTO
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                })
                .Take(count)
                .ToListAsync();

        }
    }
}
