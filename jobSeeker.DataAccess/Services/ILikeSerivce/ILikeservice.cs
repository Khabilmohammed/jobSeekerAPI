using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.ILikeSerivce
{
    public interface ILikeservice
    {
        Task<LikeDTO> AddLikeAsync(Like like);
        Task<bool> RemoveLikeAsync(int postId, string userId);
        Task<IEnumerable<Like>> GetLikesForPostAsync(int postId);

        Task<int> GetLikesCountForPostAsync(int postId);
    }
}
