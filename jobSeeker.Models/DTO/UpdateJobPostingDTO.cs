using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class UpdateJobPostingDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string ExperienceRequired { get; set; }
        public string Skills { get; set; }
        public string SalaryRange { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string JobType { get; set; }
    }
}
