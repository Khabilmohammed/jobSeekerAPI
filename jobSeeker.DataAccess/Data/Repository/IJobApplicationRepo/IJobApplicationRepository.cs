using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IJobApplicationRepo
{
    public interface IJobApplicationRepository
    {
        Task<JobApplication> CreateJobApplicationAsync(JobApplication jobApplication);
        Task<JobApplication> GetJobApplicationByIdAsync(int jobApplicationId);
        Task<IEnumerable<JobApplication>> GetAllApplicationsForJobPostingAsync(int jobPostingId);
        Task<IEnumerable<JobApplication>> GetUserApplicationsAsync(string userId);
        Task<bool> UpdateJobApplicationStatusAsync(int jobApplicationId, string status);
        Task<bool> DeleteJobApplicationAsync(int jobApplicationId);
    }
}
