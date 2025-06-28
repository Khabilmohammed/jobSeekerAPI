using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace jobSeeker.DataAccess.Data.Repository.IJobPostingRepo
{
    public class JobPostingRepository:IJobPostingRepository
    {
        private readonly ApplicationDbContext _context;
        public JobPostingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<JobPosting> AddJobPostingAsync(JobPosting jobPosting)
        {
            await _context.JobPostings.AddAsync(jobPosting);
            await _context.SaveChangesAsync();
            return jobPosting;
        }
        public async Task<IEnumerable<JobPosting>> GetAllJobPostingsAsync(string? location, string? jobType, string? experience)
        {
            var currentDate = DateTime.UtcNow;
            var query = _context.JobPostings
           .Include(post => post.Company)
           .Where(post => post.ExpiryDate > currentDate)
           .AsQueryable();

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(post => post.Location.Contains(location));
            }

            if (!string.IsNullOrEmpty(jobType))
            {
                query = query.Where(post => post.JobType == jobType);
            }

            if (!string.IsNullOrEmpty(experience))
            {
                query = query.Where(post => post.ExperienceRequired.Contains(experience));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<JobPosting>> GetAllJobPostingsAdminAsync()
        {
            return await _context.JobPostings.Include(jp => jp.Company).OrderByDescending(jp => jp.JobId).ToListAsync();
        }

        public async Task<JobPosting?> GetJobPostingByIdAsync(int jobId)
        {
            return await _context.JobPostings
         .Include(jp => jp.Company) 
         .FirstOrDefaultAsync(jp => jp.JobId == jobId);
        }
        public async Task<IEnumerable<JobPosting>> GetCompanyJobPostingsAsync(int companyId)
        {
            return await _context.JobPostings
                .Where(jp => jp.CompanyId == companyId)
                .OrderByDescending(jp => jp.JobId) 
                .ToListAsync();
        }
        public async Task<bool> UpdateJobPostingAsync(JobPosting jobPosting)
        {
            _context.JobPostings.Update(jobPosting);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteJobPostingAsync(int jobId)
        {
            var jobPosting = await _context.JobPostings.FindAsync(jobId);
            if (jobPosting == null) return false;
            _context.JobPostings.Remove(jobPosting);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<JobPostingDTO>> SearchJobPostsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<JobPostingDTO>();
            }

            query = query.ToLower(); // Normalize query for case-insensitive search
            var currentDate = DateTime.UtcNow;
            var jobPostings = await _context.JobPostings
                .Where(jp =>
                    jp.Title.ToLower().Contains(query) ||
                    jp.Description.ToLower().Contains(query) ||
                    jp.Location.ToLower().Contains(query))
                .Where(jp => jp.ExpiryDate > currentDate)
                .Select(jp => new JobPostingDTO
                {
                    JobId = jp.JobId,
                    Title = jp.Title,
                    Description = jp.Description,
                    Location = jp.Location,
                    PostedDate = jp.PostedDate,
                    CompanyId=jp.CompanyId,
                    ExperienceRequired=jp.ExperienceRequired,
                    JobType=jp.JobType
                })
                
                .ToListAsync();

            return jobPostings;
        }
    }
}
