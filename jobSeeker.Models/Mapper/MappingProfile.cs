using AutoMapper;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes))
                .ForMember(dest => dest.Shares, opt => opt.MapFrom(src => src.Shares))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();

            CreateMap<CreatePostDTO, Post>()
             .ForMember(dest => dest.Images, opt => opt.Ignore()); // **Ignore Images Mapping**

            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Like, LikeDTO>().ReverseMap();
            CreateMap<Share, ShareDTO>().ReverseMap();
            CreateMap<PostImage, PostImageDTO>().ReverseMap();
        }
    }
}
