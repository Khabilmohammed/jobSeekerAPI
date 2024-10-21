using jobSeeker.Models.DTO;
using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.ILikeRepo
{
    public interface ILikeRepository
    {
        Task<Like> AddLikeAsync(Like like);
        Task<bool> RemoveLikeAsync(int likeId);
        Task<Like> GetLikeByIdAsync(int likeId);
        Task<IEnumerable<Like>> GetLikesForPostAsync(int postId);
        Task<IEnumerable<LikeDTO>> GetLikesByUserIdAsync(string userId);
        Task<Like> GetLikeByUserAndPostAsync(string userId, int postId);
        Task<int> GetLikesCountForPostAsync(int postId);
        Task SaveChangesAsync();
    }
}
