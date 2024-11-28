using jobSeeker.DataAccess.Data.Repository.IFollowRepo;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IFollowService
{
    public class FollowServices:IFollowServices
    {
        private readonly IFollowRepository _followRepository;
        public FollowServices(IFollowRepository followRepository)
        {
            _followRepository = followRepository;
        }
        public async Task<bool> FollowAsync(FollowDTO followDto)
        {
            return await _followRepository.AddFollowAsync(followDto.FollowerId, followDto.FollowingId);
        }

        public async Task<bool> UnfollowAsync(FollowDTO followDto)
        {
            return await _followRepository.RemoveFollowAsync(followDto.FollowerId, followDto.FollowingId);
        }

        public async Task<List<followUserdetailDTO>> GetFollowersAsync(string userId)
        {
            return await _followRepository.GetFollowersAsync(userId);
        }

        public async Task<List<followUserdetailDTO>> GetFollowingAsync(string userId)
        {
            return await _followRepository.GetFollowingAsync(userId);
        }
        public async Task<bool> IsFollowingAsync(string followerId, string followingId)
        {
            return await _followRepository.IsFollowingAsync(followerId, followingId);
        }

    }
}
