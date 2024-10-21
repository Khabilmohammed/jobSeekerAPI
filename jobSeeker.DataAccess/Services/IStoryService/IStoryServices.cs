using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IStoryService
{
    public interface IStoryServices
    {

        Task<StoryDTO> AddStoryAsync(CreateStoryDTO storyDto);
        Task<IEnumerable<StoryDTO>> GetAllActiveStoriesAsync();
        Task<IEnumerable<StoryDTO>> GetStoriesByUserIdAsync(string userId);
        Task<bool> RemoveStoryAsync(int storyId); // Use int for storyId

        Task<StoryDTO> GetStoryByIdAsync(int storyId);
        Task<IEnumerable<StoryDTO>> GetAllStoriesAsync();
        Task MarkInactiveStoriesAsync();
        Task<IEnumerable<StoryDTO>> GetStoriesFromOthersAsync(string userId);
        Task<IEnumerable<StoryDTO>> GetArchivedStoriesAsync(string userId);

    }
}
