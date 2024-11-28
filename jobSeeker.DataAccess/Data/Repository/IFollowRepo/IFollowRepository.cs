using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IFollowRepo
{
    public interface IFollowRepository
    {
        Task<bool> AddFollowAsync(string followerId, string followingId);
        Task<bool> RemoveFollowAsync(string followerId, string followingId);
        Task<List<followUserdetailDTO>> GetFollowersAsync(string userId);
        Task<List<followUserdetailDTO>> GetFollowingAsync(string userId);
        Task<bool> IsFollowingAsync(string followerId, string followingId);
    }
}
