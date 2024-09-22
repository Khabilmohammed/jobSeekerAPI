using jobSeeker.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IPostService
{
    public interface IPostServices
    {
        //post realted method
        Task<Post> CreatePostAsync(Post post);
        Task<Post> GetPostByIdAsync(int postId);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(int postId);


        //comment related method
        Task<Comment> AddCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int commentId);

        // like rekated method
        Task<Like> AddLikeAsync(Like like);
        Task<bool> RemoveLikeAsync(int postId, string userId);


        Task<Share> AddShareAsync(Share share);


        //image related method
        Task<PostImage> AddImageAsync(PostImage postImage);
        Task<PostImage> AddImageAsync(IFormFile imageFile, int postId);
        Task<bool> RemoveImageAsync(int imageId);

        Task<string> UploadImageAsync(IFormFile imageFile, int postId);

    }
}
