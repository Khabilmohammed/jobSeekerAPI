using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IFollowService
{
    public interface IFollowServices
    {
        Task<bool> FollowAsync(FollowDTO followDto);
        Task<bool> UnfollowAsync(FollowDTO followDto);
        Task<List<followUserdetailDTO>> GetFollowersAsync(string userId);
        Task<List<followUserdetailDTO>> GetFollowingAsync(string userId);
        Task<bool> IsFollowingAsync(string followerId, string followingId);

    }
}
