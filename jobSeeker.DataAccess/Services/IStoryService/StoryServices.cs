using AutoMapper;
using jobSeeker.DataAccess.Data.Repository.IStoryRepo;
using jobSeeker.Models.DTO;
using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jobSeeker.DataAccess.Data.Repository.IUserRepository;
using Microsoft.EntityFrameworkCore;

namespace jobSeeker.DataAccess.Services.IStoryService
{
    public class StoryServices : IStoryServices
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public StoryServices(IStoryRepository storyRepository, 
            IMapper mapper, IUserRepository userRepository)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

       
        public async Task<StoryDTO> AddStoryAsync(CreateStoryDTO storyDto)
        {
            var storyEntity = _mapper.Map<Story>(storyDto);
            storyEntity.ImageUrl = storyDto.ImageUrl;
            storyEntity.IsActive = true;
            storyEntity.CreatedAt = DateTime.UtcNow;
            storyEntity.ExpirationTime = DateTime.UtcNow.AddHours(24);
            var createdStory = await _storyRepository.CreateStoryAsync(storyEntity);
            var user = await _userRepository.GetUserByIdAsync(storyEntity.UserId);
            var storyDtoResult = _mapper.Map<StoryDTO>(createdStory);
            storyDtoResult.UserName = user?.UserName; 
            return storyDtoResult;
        }

        public async Task<IEnumerable<StoryDTO>> GetAllStoriesAsync()
        {
            var stories = await _storyRepository.GetAllStoriesAsync(); 
            return _mapper.Map<IEnumerable<StoryDTO>>(stories); 
        }

        public async Task<IEnumerable<StoryDTO>> GetAllActiveStoriesAsync()
        {
            var stories = await _storyRepository.GetActiveStoriesAsync();
            return _mapper.Map<IEnumerable<StoryDTO>>(stories);
        }

        public async Task<IEnumerable<StoryDTO>> GetStoriesByUserIdAsync(string userId)
        {
            var stories = await _storyRepository.GetStoriesByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<StoryDTO>>(stories);
        }


        public async Task<bool> RemoveStoryAsync(int storyId)
        {
            return await _storyRepository.DeleteStoryAsync(storyId);
        }

        public async Task<StoryDTO> GetStoryByIdAsync(int storyId)
        {
            var story = await _storyRepository.GetStoryByIdAsync(storyId);
            return _mapper.Map<StoryDTO>(story);
        }

        public async Task MarkInactiveStoriesAsync()
        {
            // Get the threshold for marking stories as inactive (e.g., 24 hours ago)
            DateTime threshold = DateTime.UtcNow.AddHours(-24);

            // Fetch stories that need to be marked as inactive
            var inactiveStories = await _storyRepository.GetInactiveStoriesAsync(threshold);

            if (inactiveStories.Any())
            {
                foreach (var story in inactiveStories)
                {
                    story.IsActive = false; // Mark the story as inactive
                }

                await _storyRepository.SaveChangesAsync(); // Save changes to the database
            }
        }

    }
}
