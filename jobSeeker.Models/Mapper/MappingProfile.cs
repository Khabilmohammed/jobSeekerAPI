﻿using AutoMapper;
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
             .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<UpdatePostDTO, Post>()
          .ForMember(dest => dest.Images, opt => opt.Ignore()); 

          CreateMap<Comment, CommentDTO>().ReverseMap();




            CreateMap<Like, LikeDTO>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))  // Map UserName from the related User entity
           .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))  // Map UserId directly from the Like entity
           .ReverseMap();


            CreateMap<Like, LikeCreateDTO>().ReverseMap();
            CreateMap<Share, ShareDTO>().ReverseMap();
            CreateMap<PostImage, PostImageDTO>().ReverseMap();


            CreateMap<Story, StoryDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ReverseMap();

            CreateMap<CreateStoryDTO, Story>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<Comment, CommentResponseDTO>()
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                        .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
                        .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                        .ReverseMap();
        }
    }
}
