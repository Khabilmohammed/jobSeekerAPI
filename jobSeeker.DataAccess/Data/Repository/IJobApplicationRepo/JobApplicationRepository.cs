using jobSeeker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IJobApplicationRepo
{
    public class JobApplicationRepository:IJobApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        public JobApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobApplication> CreateJobApplicationAsync(JobApplication jobApplication)
        {
            var jobPosting = await _context.JobPostings
     .Include(jp => jp.Company) 
     .FirstOrDefaultAsync(jp => jp.JobId == jobApplication.JobPostingId);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == jobApplication.UserId);

            if (jobPosting == null || user == null)
            {
                
                throw new Exception("Job Posting or User not found.");
            }

            jobApplication.JobPosting = jobPosting;
            jobApplication.User = user;

            await _context.JobApplications.AddAsync(jobApplication);
            await _context.SaveChangesAsync();
            return jobApplication;
        }

        public async Task<JobApplication> GetJobApplicationByIdAsync(int jobApplicationId)
        {
            return await _context.JobApplications
                .Include(ja => ja.JobPosting)
                .Include(ja => ja.User)
                .FirstOrDefaultAsync(ja => ja.JobApplicationId == jobApplicationId);
        }
        public async Task<IEnumerable<JobApplication>> GetUserApplicationsAsync(string userId)
        {
            return await _context.JobApplications
                .Where(ja => ja.UserId == userId)
                .OrderByDescending(ja => ja.ApplicationDate)
                .Include(ja => ja.JobPosting)
                .ToListAsync();
        }

        public async Task<bool> UpdateJobApplicationStatusAsync(int jobApplicationId, string status)
        {
            var application = await _context.JobApplications.FindAsync(jobApplicationId);
            if (application == null) return false;

            application.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteJobApplicationAsync(int jobApplicationId)
        {
            var application = await _context.JobApplications.FindAsync(jobApplicationId);
            if (application == null) return false;

            _context.JobApplications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<JobApplication>> GetAllApplicationsForJobPostingAsync(int jobPostingId)
        {
            return await _context.JobApplications
                .Where(ja => ja.JobPostingId == jobPostingId)
                .ToListAsync();
        }

    }
}
