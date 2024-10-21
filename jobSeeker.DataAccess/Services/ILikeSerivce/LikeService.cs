using jobSeeker.DataAccess.Data.Repository.ILikeRepo;
using jobSeeker.DataAccess.Data.Repository.IUserRepository;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.ILikeSerivce
{
    public class LikeService:ILikeservice
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserRepository _userRepository;
        public LikeService(ILikeRepository likeRepository, 
            IUserRepository userRepository)
        {
            _likeRepository = likeRepository;
            _userRepository = userRepository;
        }
        public async Task<LikeDTO> AddLikeAsync(Like like)
        {
            try
            {
                // Check if the like already exists for the user and post
                var existingLike = await _likeRepository.GetLikeByUserAndPostAsync(like.UserId, like.PostId);

                if (existingLike != null)
                {
                    // Log the conflict
                    Console.WriteLine($"Like already exists for UserId: {like.UserId} and PostId: {like.PostId}");
                    return null; // Like already exists, return null to indicate failure
                }

                // Add the new like
                await _likeRepository.AddLikeAsync(like);
                await _likeRepository.SaveChangesAsync();

                // Fetch the user details from the user repository
                var user = await _userRepository.GetUserByIdAsync(like.UserId); // Fetch user info

                // Log the user and like information
                Console.WriteLine($"User found: {user?.UserName}, LikeId: {like.LikeId}");

                // Map the Like entity to LikeDTO
                var likeDto = new LikeDTO
                {
                    LikeId = like.LikeId,  // This will be automatically generated after adding the like
                    PostId = like.PostId,
                    UserId = like.UserId,
                    UserName = user?.UserName // Fetch UserName from the user entity
                };

                return likeDto; // Return the DTO with the like details
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism should be in place)
                Console.WriteLine($"Error adding like: {ex.Message}");
                return null; // Indicate failure
            }
        }



        public async Task<bool> RemoveLikeAsync(int postId, string userId)
        {
            try
            {
                var likeToRemove = await _likeRepository.GetLikeByUserAndPostAsync(userId, postId);

                if (likeToRemove == null)
                {
                    return false;
                }

                await _likeRepository.RemoveLikeAsync(likeToRemove.LikeId);
                await _likeRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                return false;
            }
        }

        public async Task<int> GetLikesCountForPostAsync(int postId)
        {
            // Call the repository method to get the like count for a specific post
            return await _likeRepository.GetLikesCountForPostAsync(postId);
        }


        public async Task<IEnumerable<Like>> GetLikesForPostAsync(int postId)
        {
            return await _likeRepository.GetLikesForPostAsync(postId);
        }
    }
}
