using jobSeeker.DataAccess.Data.Repository.ISavedRepo;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.ISavedPostService
{
    public class SavedPostservice : ISavedPostservices
    {
        private readonly ISavedPostRepository _savedPostRepository;

        public SavedPostservice(ISavedPostRepository savedPostRepository)
        {
            _savedPostRepository = savedPostRepository;
        }

        public async Task<bool> SavePostAsync(string userId, int postId) // Updated to accept userId and postId
        {
            // Check if the post is already saved by the user
            var exists = await _savedPostRepository.SavedPostExistsAsync(userId, postId);
            if (exists)
            {
                return false; // Post already saved
            }

            // Create a new saved post entry
            var savedPost = new SavedPost
            {
                UserId = userId,
                PostId = postId,
               
            };

            // Add the saved post to the repository and save changes
            await _savedPostRepository.AddSavedPostAsync(savedPost);
            await _savedPostRepository.SaveChangesAsync();

            return true; // Successfully saved the post
        }

        public async Task<bool> RemoveSavedPostAsync(string userId, int postId)
        {
            var savedPost = await _savedPostRepository.GetSavedPostAsync(userId, postId);
            if (savedPost == null)
            {
                return false; // Saved post not found
            }

            await _savedPostRepository.RemoveSavedPostAsync(savedPost);
            await _savedPostRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SavedPost>> GetUserSavedPostsAsync(string userId)
        {
            return await _savedPostRepository.GetUserSavedPostsAsync(userId);
        }
    }

}
