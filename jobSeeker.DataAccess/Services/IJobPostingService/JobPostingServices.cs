using AutoMapper;
using jobSeeker.DataAccess.Data.Repository.IJobPostingRepo;
using jobSeeker.Models.DTO;
using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace jobSeeker.DataAccess.Services.IJobPostingService
{
    public class JobPostingServices:IJobPostingServices
    {
        private readonly IJobPostingRepository _repository;
        private readonly IMapper _mapper;
        public JobPostingServices(IJobPostingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<JobPostingDTO> CreateJobPostingAsync(CreateJobPostingDTO createJobPostingDTO)
        {
            var jobPosting = _mapper.Map<JobPosting>(createJobPostingDTO);
            var createdJobPosting = await _repository.AddJobPostingAsync(jobPosting);
            return _mapper.Map<JobPostingDTO>(createdJobPosting);
        }
        public async Task<IEnumerable<JobPostingDTO>> GetAllJobPostingsAsync(string? location, string? jobType, string? experience)
        {
            var jobPostings = await _repository.GetAllJobPostingsAsync(location, jobType, experience);
            return jobPostings.Select(post => new JobPostingDTO
            {
                JobId = post.JobId,
                CompanyId = post.CompanyId,
                Title = post.Title,
                Description = post.Description,
                Location = post.Location,
                ExperienceRequired = post.ExperienceRequired,
                Skills = post.Skills,
                SalaryRange = post.SalaryRange,
                PostedDate = post.PostedDate,
                ExpiryDate = post.ExpiryDate,
                JobType = post.JobType,
                IsActive = post.IsActive,
                LogoUrl = post.Company?.LogoUrl // Map LogoUrl from the Company navigation property
            }).ToList();
        }

        public async Task<IEnumerable<JobPostingDTO>> GetAllJobPostingsAdminAsync()
        {
            var jobPostings = await _repository.GetAllJobPostingsAdminAsync();
            return _mapper.Map<IEnumerable<JobPostingDTO>>(jobPostings);
        }

        public async Task<JobPostingDTO?> GetJobPostingByIdAsync(int jobId)
        {
            var jobPosting = await _repository.GetJobPostingByIdAsync(jobId);
            return jobPosting != null ? _mapper.Map<JobPostingDTO>(jobPosting) : null;
        }
        public async Task<IEnumerable<JobPostingDTO>> GetCompanyJobPostingsAsync(int companyId)
        {
            var jobPostings = await _repository.GetCompanyJobPostingsAsync(companyId);
            return _mapper.Map<IEnumerable<JobPostingDTO>>(jobPostings);
        }

        public async Task<bool> UpdateJobPostingAsync(int jobId, UpdateJobPostingDTO createJobPostingDTO)
        {
            var jobPosting = await _repository.GetJobPostingByIdAsync(jobId);
            if (jobPosting == null) return false;

            _mapper.Map(createJobPostingDTO, jobPosting);
            return await _repository.UpdateJobPostingAsync(jobPosting);
        }
        public async Task<bool> DeleteJobPostingAsync(int jobId)
        {
            return await _repository.DeleteJobPostingAsync(jobId);
        }

        public async Task<IEnumerable<JobPostingDTO>> SearchJobPostsAsync(string query)
        {
            return await _repository.SearchJobPostsAsync(query);
        }
    }
}
