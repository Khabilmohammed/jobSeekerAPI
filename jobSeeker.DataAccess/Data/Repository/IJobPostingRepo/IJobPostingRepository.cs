using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IJobPostingRepo
{
    public interface IJobPostingRepository
    {
        Task<JobPosting> AddJobPostingAsync(JobPosting jobPosting);
        Task<IEnumerable<JobPosting>> GetAllJobPostingsAsync(string? location, string? jobType, string? experience);
        Task<IEnumerable<JobPosting>> GetAllJobPostingsAdminAsync();
        Task<IEnumerable<JobPosting>> GetCompanyJobPostingsAsync(int companyId);
        Task<JobPosting?> GetJobPostingByIdAsync(int jobId);
        Task<bool> UpdateJobPostingAsync(JobPosting jobPosting);
        Task<bool> DeleteJobPostingAsync(int jobId);
        Task<IEnumerable<JobPostingDTO>> SearchJobPostsAsync(string query);
    }
}
