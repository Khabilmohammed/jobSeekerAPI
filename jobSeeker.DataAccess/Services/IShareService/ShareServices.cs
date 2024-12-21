using AutoMapper;
using jobSeeker.DataAccess.Data.Repository.IShareRepo;
using jobSeeker.DataAccess.Data.Repository.IUserRepository;
using jobSeeker.DataAccess.Migrations;
using jobSeeker.DataAccess.Services.IPostService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IShareService
{
    public class ShareServices:IShareServices
    {
        private readonly IPostServices _postService;
        private readonly IShareRepository _shareRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public ShareServices(IShareRepository shareRepository, IPostServices postService, IMapper mapper,
            IUserRepository userRepository)
        {
            _shareRepository = shareRepository;
            _postService = postService;
            _mapper = mapper;
            _userRepository = userRepository;

        }
        public async Task<ShareDTO> SharePostAsync(CreateShareDTO shareDTO)
        {
            var post = await _postService.GetPostByIdAsync(shareDTO.PostId);
            var sender = await _userRepository.GetUserByIdAsync(shareDTO.SenderId);
            var recipient = await _userRepository.GetUserByIdAsync(shareDTO.RecipientId);
            if (post == null || sender == null || recipient == null)
            {
                return null; // Could return a more specific result or throw an exception
            }

            var share = new Share
            {
                PostId = shareDTO.PostId,
                SenderId = shareDTO.SenderId,
                RecipientId = shareDTO.RecipientId,
            };

            var createdShare = await _shareRepository.AddAsync(share);
            return new ShareDTO
            {
                ShareId = createdShare.ShareId,
                PostId = createdShare.PostId,
                SenderId = createdShare.SenderId,
                RecipientId = createdShare.RecipientId,
                SharedAt = createdShare.SharedAt
            };
        }

        public async Task<IEnumerable<Share>> GetUserSharedPostsAsync(string userId)
        {
            return await _shareRepository.GetSharesByUserIdAsync(userId);
        }

    }
}
