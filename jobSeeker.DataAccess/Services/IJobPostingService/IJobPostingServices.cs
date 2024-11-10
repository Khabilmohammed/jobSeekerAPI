﻿using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IJobPostingService
{
    public interface IJobPostingServices
    {
        Task<JobPostingDTO> CreateJobPostingAsync(CreateJobPostingDTO createJobPostingDTO);
        Task<IEnumerable<JobPostingDTO>> GetAllJobPostingsAsync();
        Task<JobPostingDTO?> GetJobPostingByIdAsync(int jobId);
        Task<IEnumerable<JobPostingDTO>> GetCompanyJobPostingsAsync(int companyId);
        Task<bool> UpdateJobPostingAsync(int jobId, CreateJobPostingDTO createJobPostingDTO);
        Task<bool> DeleteJobPostingAsync(int jobId);
    }
}