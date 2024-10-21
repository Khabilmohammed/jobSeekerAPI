using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IStoryRepo
{
    public class StoryRepository: IStoryRepository
    {
        private readonly ApplicationDbContext _context;

        public StoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Story> CreateStoryAsync(Story story)
        {
            try
            {
                await _context.Stories.AddAsync(story);
                await _context.SaveChangesAsync();
                return story;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating story", ex);
            }
        }

        public async Task<IEnumerable<Story>> GetAllStoriesAsync()
        {
            return await _context.Stories
                            .Include(s => s.User) // Include the user information
                            .ToListAsync();
        }

        public async Task<IEnumerable<Story>> GetActiveStoriesAsync()
        {
            try
            {
                return await _context.Stories
                    .Where(s => s.CreatedAt >= DateTime.UtcNow.AddHours(-24)) // Stories active for the last 24 hours
                    .Include(s => s.User) // Include the related user
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching active stories", ex);
            }
        }

        public async Task<IEnumerable<StoryDTO>> GetStoriesByUserIdAsync(string userId)
        {
            try
            {
                DateTime last24Hours = DateTime.UtcNow.AddHours(-24);

                return await _context.Stories
                    .Include(s => s.User) 
                    .Where(s => s.UserId == userId &&  s.CreatedAt >= last24Hours) // Filter for active stories within 24 hours
                    .Select(s => new StoryDTO
                    {
                        StoryId = s.StoryId,
                        UserId = s.UserId,
                        ImageUrl = s.ImageUrl,
                        CreatedAt = s.CreatedAt,
                        IsActive = s.IsActive,
                        UserName = s.User.UserName 
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching stories for user {userId}", ex);
            }
        }

        public async Task<IEnumerable<StoryDTO>> GetArchivedStoriesAsync(string userId)
        {
            try
            {
                DateTime expirationThreshold = DateTime.UtcNow.AddHours(-24);

                // Fetch stories older than 24 hours (i.e., archived/expired stories)
                return await _context.Stories
                    .Include(s => s.User)
                    .Where(s => s.UserId == userId && s.CreatedAt < expirationThreshold) // Archived stories
                    .Select(s => new StoryDTO
                    {
                        StoryId = s.StoryId,
                        UserId = s.UserId,
                        ImageUrl = s.ImageUrl,
                        CreatedAt = s.CreatedAt,
                        IsActive = s.IsActive,
                        UserName = s.User.UserName
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching archived stories for user {userId}", ex);
            }
        }



        public async Task<bool> DeleteStoryAsync(int storyId)
        {
            try
            {
                var story = await _context.Stories.FindAsync(storyId);
                if (story == null)
                    return false;

                _context.Stories.Remove(story);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting story with ID {storyId}", ex);
            }
        }

        public async Task<Story> GetStoryByIdAsync(int storyId)
        {
            try
            {
                return await _context.Stories
             .Include(s => s.User) 
             .FirstOrDefaultAsync(s => s.StoryId == storyId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching story with ID {storyId}", ex);
            }
        }
        public async Task<IEnumerable<Story>> GetInactiveStoriesAsync(DateTime threshold)
        {
            return await _context.Stories
                .Where(s => s.CreatedAt < threshold && s.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Story>> GetStoriesFromOthersAsync(string userId)
        {
            return await _context.Stories
                .Include(s => s.User)
                .Where(s => s.UserId != userId && s.CreatedAt >= DateTime.UtcNow.AddHours(-24)) // Exclude user's own stories and only active stories
                .ToListAsync();
          
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
