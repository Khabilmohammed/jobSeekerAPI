using jobSeeker.DataAccess.Data;
using jobSeeker.DataAccess.Services.CloudinaryService;
using jobSeeker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IPostService
{
    public class PostServices : IPostServices
    {
        private readonly ApplicationDbContext _db;
        private readonly CloudinaryServices _cloudinaryServices;
        private readonly ILogger<PostServices> _logger;
        public PostServices(ApplicationDbContext db, CloudinaryServices cloudinaryServices,
            ILogger<PostServices> logger)
        {
            _db = db;
            _cloudinaryServices = cloudinaryServices;
            _logger = logger;
        }
        public async Task<Post> CreatePostAsync(Post post)
        {
            try
            {
                _db.Posts.Add(post);
                await _db.SaveChangesAsync();
                return post;
            }
            catch (DbUpdateException ex)
            {
                // Log the detailed error message
                var innerException = ex.InnerException?.Message;
                // Handle or log the exception as needed
                throw new Exception($"An error occurred while saving changes: {innerException}", ex);
            }
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            return await _db.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Shares)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.PostId == postId);
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _db.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Shares)
                .Include(p => p.Images)
                .ToListAsync();
        }


        public async Task<Post> UpdatePostAsync(Post post)
        {
            _db.Posts.Update(post);
            await _db.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            var post = await _db.Posts.Include(p => p.Images).FirstOrDefaultAsync(p => p.PostId == postId);
            if (post != null)
            {
                // Delete all images associated with the post
                _db.PostImages.RemoveRange(post.Images);

                // Delete the post itself
                _db.Posts.Remove(post);

                return await _db.SaveChangesAsync() > 0;
            }
            return false;
        }

        // Additional methods for handling comments, likes, shares, and images
        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var comment = await _db.Comments.FindAsync(commentId);
            if (comment != null)
            {
                _db.Comments.Remove(comment);
                return await _db.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Like> AddLikeAsync(Like like)
        {
            _db.Likes.Add(like);
            await _db.SaveChangesAsync();
            return like;
        }
        public async Task<bool> RemoveLikeAsync(int postId, string userId)
        {
            var like = await _db.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            if (like != null)
            {
                _db.Likes.Remove(like);
                return await _db.SaveChangesAsync() > 0;
            }
            return false;
        }



        public async Task<Share> AddShareAsync(Share share)
        {
            _db.Shares.Add(share);
            await _db.SaveChangesAsync();
            return share;
        }

        public async Task<PostImage> AddImageAsync(IFormFile imageFile, int postId)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Invalid image file.");
            }

            try
            {
                _logger.LogInformation("Starting image upload for PostId: {PostId}", postId);

                // Upload the image to Cloudinary
                var uploadResult = await _cloudinaryServices.UploadImageAsync(imageFile);

                if (uploadResult == null || string.IsNullOrEmpty(uploadResult.SecureUrl.ToString()))
                {
                    _logger.LogError("Image upload failed for PostId: {PostId}", postId);
                    throw new InvalidOperationException("Image upload failed.");
                }

                var imageUrl = uploadResult.SecureUrl.ToString();

                // Check if the image already exists for the post to avoid duplicates
                var existingImage = await _db.PostImages
                    .FirstOrDefaultAsync(img => img.ImageUrl == imageUrl && img.PostId == postId);

                if (existingImage != null)
                {
                    _logger.LogInformation("Image already exists for PostId: {PostId}", postId);
                    return existingImage; // Return the existing image instead of adding it again
                }

                var postImage = new PostImage
                {
                    PostId = postId,
                    ImageUrl = imageUrl
                };

                _db.PostImages.Add(postImage);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Image added successfully for PostId: {PostId}", postId);

                return postImage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image for PostId: {PostId}", postId);
                throw new Exception($"An error occurred while uploading the image: {ex.Message}");
            }
        }



        public async Task<bool> RemoveImageAsync(int imageId)
        {
            var image = await _db.PostImages.FindAsync(imageId);
            if (image != null)
            {
                _db.PostImages.Remove(image);
                return await _db.SaveChangesAsync() > 0;
            }
            return false;
        }

       
    }
}
