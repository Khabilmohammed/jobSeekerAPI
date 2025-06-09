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
             try
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
            catch (Exception ex)
            {
                throw new Exception("Error while adding story.", ex);
            }
        }

        public async Task<IEnumerable<StoryDTO>> GetAllStoriesAsync()
        {
            try
            {
                var stories = await _storyRepository.GetAllStoriesAsync();
                return _mapper.Map<IEnumerable<StoryDTO>>(stories);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving all stories.", ex);
            }
        }

        public async Task<IEnumerable<StoryDTO>> GetAllActiveStoriesAsync()
        {
            try
            {
                var stories = await _storyRepository.GetActiveStoriesAsync();
                return _mapper.Map<IEnumerable<StoryDTO>>(stories);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving active stories.", ex);
            }
        }

        public async Task<IEnumerable<StoryDTO>> GetStoriesByUserIdAsync(string userId)
        {
            try
            {
                var stories = await _storyRepository.GetStoriesByUserIdAsync(userId);
                return _mapper.Map<IEnumerable<StoryDTO>>(stories);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving stories by user ID.", ex);
            }
        }

        public async Task<IEnumerable<StoryDTO>> GetArchivedStoriesAsync(string userId)
        {
            try
            {
                var stories = await _storyRepository.GetArchivedStoriesAsync(userId);
                return _mapper.Map<IEnumerable<StoryDTO>>(stories);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving archived stories.", ex);
            }
        }

        public async Task<bool> RemoveStoryAsync(int storyId)
        {
            try
            {
                return await _storyRepository.DeleteStoryAsync(storyId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while removing story.", ex);
            }
        }

        public async Task<StoryDTO> GetStoryByIdAsync(int storyId)
        {
            try
            {
                var story = await _storyRepository.GetStoryByIdAsync(storyId);
                return _mapper.Map<StoryDTO>(story);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving story by ID.", ex);
            }
        }

        public async Task<IEnumerable<StoryDTO>> GetStoriesFromOthersAsync(string userId)
        {
            try
            {
                var stories = await _storyRepository.GetStoriesFromOthersAsync(userId);
                return _mapper.Map<IEnumerable<StoryDTO>>(stories);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving stories from other users.", ex);
            }
        }


        public async Task MarkInactiveStoriesAsync()
        {
           try
            {
                DateTime threshold = DateTime.UtcNow.AddHours(-24);
                var inactiveStories = await _storyRepository.GetInactiveStoriesAsync(threshold);

                if (inactiveStories.Any())
                {
                    foreach (var story in inactiveStories)
                    {
                        story.IsActive = false;
                    }

                    await _storyRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while marking stories as inactive.", ex);
            }
        }

    }
}
