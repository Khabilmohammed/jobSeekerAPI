using jobSeeker.Models.DTO;
using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IJobApplicationService
{
    public interface IJobApplicationServices
    {
        Task<JobApplication> ApplyForJobAsync(CreateJobApplicationDTO jobApplicationDto);
        Task<JobApplication> GetApplicationByIdAsync(int jobApplicationId);
        Task<IEnumerable<JobApplication>> GetApplicationsForJobPostingAsync(int jobPostingId);
        Task<IEnumerable<JobApplication>> GetUserApplicationsAsync(string userId);
        Task<bool> ChangeApplicationStatusAsync(int jobApplicationId, string status);
        Task<bool> RemoveApplicationAsync(int jobApplicationId);
    }
}
