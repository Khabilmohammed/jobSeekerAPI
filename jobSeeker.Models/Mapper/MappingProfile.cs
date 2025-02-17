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
            CreateMap<Share, ShareDTO>()
           
    .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Post.PostId))
                .ReverseMap();

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

            CreateMap<SavedPost, SavePostDTO>()
                  .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Post.Likes.Count))
    .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Post.Comments.Count))
    .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Post.Images.Select(img => img.ImageUrl)))
    .ForMember(dest => dest.PostContent, opt => opt.MapFrom(src => src.Post.Content))
    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Post.CreatedAt))
           .ReverseMap();

            CreateMap<Experience, ExperienceDto>()
          .ReverseMap();

            CreateMap<CreateExperienceDto, Experience>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<Certificate, CertificateDto>().ReverseMap();
            CreateMap<CreateCertificateDto, Certificate>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<Education, CreateEducationDTO>().ReverseMap();
            CreateMap<Education, EducationResponseDTO>().ReverseMap()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            // Company and CompanyDTO mapping
            CreateMap<Company, CompanyDTO>().ReverseMap();

            // CreateCompanyDTO to Company
            CreateMap<CreateCompanyDTO, Company>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ReverseMap();

            // UpdateCompanyDTO to Company
            CreateMap<UpdateCompanyDTO, Company>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                  .ForMember(dest => dest.LogoUrl, opt => opt.Ignore()) 
                  .ForMember(dest => dest.Banner, opt => opt.Ignore()) 
                    .ReverseMap();

            CreateMap<JobPosting, JobPostingDTO>()
                 .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.Company.LogoUrl))
                .ReverseMap();

            CreateMap<CreateJobPostingDTO, JobPosting>();
 
            CreateMap<UpdateJobPostingDTO, JobPosting>()
    .ForMember(dest => dest.CompanyId, opt => opt.Ignore());


            CreateMap<JobApplication, JobApplicationDTO>()
         .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobPosting.Title))
         .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.JobPosting.Company.Name))
         .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
         .ForMember(dest => dest.ExpectedSalary, opt => opt.MapFrom(src => src.ExpectedSalary))
         .ForMember(dest => dest.ApplicationDate, opt => opt.MapFrom(src => src.ApplicationDate))
         .ReverseMap();

            CreateMap<CreateJobApplicationDTO, JobApplication>()
                .ForMember(dest => dest.ApplicationDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Applied"))
                .ReverseMap();

            CreateMap<Message, MessageDTO>()
           .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId))
           .ForMember(dest => dest.SenderUserName, opt => opt.MapFrom(src => src.SenderUserName))
           .ForMember(dest => dest.RecipientId, opt => opt.MapFrom(src => src.RecipientId))
           .ForMember(dest => dest.RecipientUserName, opt => opt.MapFrom(src => src.RecipientUserName))
           .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
           .ForMember(dest => dest.DateRead, opt => opt.MapFrom(src => src.DateRead))
           .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.SentAt))
           .ReverseMap();

            CreateMap<ApplicationUser, MessageUserDTO>();

        }
    }
}
