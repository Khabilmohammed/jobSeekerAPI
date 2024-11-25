using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class JobApplication
    {
        public int JobApplicationId { get; set; }
        public int JobPostingId { get; set; } 
        public string UserId { get; set; } 

        public string FullName { get; set; }
        public string Address { get; set; } 
        public decimal ExpectedSalary { get; set; } 

        public string ResumeUrl { get; set; }
        public string CoverLetter { get; set; } 
        public string Status { get; set; } 
        public DateTime ApplicationDate { get; set; }

        public JobPosting JobPosting { get; set; }
        public ApplicationUser User { get; set; }
    }
}
