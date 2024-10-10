using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IStoryRepo
{
    public interface IStoryRepository
    {
        Task<Story> CreateStoryAsync(Story story);
        Task<IEnumerable<Story>> GetActiveStoriesAsync(); 
        Task<IEnumerable<StoryDTO>> GetStoriesByUserIdAsync(string userId);
        Task<bool> DeleteStoryAsync(int storyId); 
        Task<Story> GetStoryByIdAsync(int storyId);
        Task<IEnumerable<Story>> GetAllStoriesAsync();
        Task<IEnumerable<Story>> GetInactiveStoriesAsync(DateTime threshold);
        Task SaveChangesAsync();
    }

}
