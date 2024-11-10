using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class JobPosting
    {
        [Key]
        public int JobId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [MaxLength(50)]
        public string Location { get; set; }

        [MaxLength(100)]
        public string ExperienceRequired { get; set; }

        [MaxLength(200)]
        public string Skills { get; set; }

        [MaxLength(50)]
        public string SalaryRange { get; set; }

        [Required]
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiryDate { get; set; }

        [MaxLength(50)]
        public string JobType { get; set; } // Full-time, Part-time, Contract

        public bool IsActive { get; set; } = true;
      
    }
}
