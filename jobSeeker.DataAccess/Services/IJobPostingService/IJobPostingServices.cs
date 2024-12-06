using jobSeeker.Models.DTO;
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

        Task<IEnumerable<JobPostingDTO>> GetAllJobPostingsAdminAsync();
        Task<JobPostingDTO?> GetJobPostingByIdAsync(int jobId);
        Task<IEnumerable<JobPostingDTO>> GetCompanyJobPostingsAsync(int companyId);
        Task<bool> UpdateJobPostingAsync(int jobId, UpdateJobPostingDTO createJobPostingDTO);
        Task<bool> DeleteJobPostingAsync(int jobId);
    }
}
