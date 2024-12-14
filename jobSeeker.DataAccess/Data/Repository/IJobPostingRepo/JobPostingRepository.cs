using jobSeeker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<JobPosting>> GetAllJobPostingsAsync()
        {
            var currentDate = DateTime.UtcNow; 
            var activeJobPostings = await _context.JobPostings
                 .Include(post => post.Company)
                .Where(post => post.ExpiryDate > currentDate)  
                .ToListAsync();

            return activeJobPostings;
        }

        public async Task<IEnumerable<JobPosting>> GetAllJobPostingsAdminAsync()
        {
            return await _context.JobPostings.ToListAsync();
        }

        public async Task<JobPosting?> GetJobPostingByIdAsync(int jobId)
        {
            return await _context.JobPostings.FindAsync(jobId);
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
    }
}
