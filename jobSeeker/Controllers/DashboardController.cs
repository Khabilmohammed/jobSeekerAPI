using jobSeeker.DataAccess.Data;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace jobSeeker.Controllers
{
    [Route("api/Dashboard")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var totalUsers = await _context.Users.CountAsync();
            var companyUsers = await _context.Companies.CountAsync();
            var socialMediaPosts = await _context.Posts.CountAsync();
            var jobPosts = await _context.JobPostings.CountAsync();
            var jobApplications = await _context.JobApplications.CountAsync();

            return Ok(new
            {
                totalUsers,
                companyUsers,
                socialMediaPosts,
                jobPosts,
                jobApplications
            });
        }

        [HttpGet("monthly-stats")]
        public async Task<IActionResult> GetMonthlyStats()
        {
            var stats = await GetMonthlyStatsAsync();
            return Ok(stats);
        }

        private async Task<MonthlyStatsDTO> GetMonthlyStatsAsync()
        {
            var months = Enumerable.Range(1, 12)
                .Select(i => DateTimeFormatInfo.CurrentInfo.GetMonthName(i))
                .ToList();

            var jobPosts = await _context.JobPostings
                .GroupBy(p => p.PostedDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Month, x => x.Count);

            var applications = await _context.JobApplications
                .GroupBy(a => a.ApplicationDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Month, x => x.Count);

            var companyRegistrations = await _context.Companies
                .GroupBy(c => c.CreatedAt.Month) // Ensure "CreatedAt" exists in the Companies table
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Month, x => x.Count);

            return new MonthlyStatsDTO
            {
                Months = months,
                JobPosts = months.Select((_, i) => jobPosts.ContainsKey(i + 1) ? jobPosts[i + 1] : 0).ToList(),
                Applications = months.Select((_, i) => applications.ContainsKey(i + 1) ? applications[i + 1] : 0).ToList(),
                CompanyRegistrations = months.Select((_, i) => companyRegistrations.ContainsKey(i + 1) ? companyRegistrations[i + 1] : 0).ToList(),
            };
        }

        [HttpGet("engagement-metrics")]
        public async Task<IActionResult> GetEngagementMetrics()
        {
            var metrics = await GetEngagementMetricsAsync();
            return Ok(metrics);
        }

        private async Task<EngagementMetricsDTO> GetEngagementMetricsAsync()
        {
            var months = Enumerable.Range(1, 12)
                .Select(i => DateTimeFormatInfo.CurrentInfo.GetMonthName(i))
                .ToList();

            var postsCreated = await _context.Posts
                .GroupBy(p => p.CreatedAt.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Month, x => x.Count);

            var likesReceived = await _context.Likes
                .GroupBy(l => l.LikedAt.Month) // Ensure "LikedAt" exists in the Likes table
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Month, x => x.Count);

            return new EngagementMetricsDTO
            {
                Months = months,
                PostsCreated = months.Select((_, i) => postsCreated.ContainsKey(i + 1) ? postsCreated[i + 1] : 0).ToList(),
                LikesReceived = months.Select((_, i) => likesReceived.ContainsKey(i + 1) ? likesReceived[i + 1] : 0).ToList(),
            };
        }
    }
}
