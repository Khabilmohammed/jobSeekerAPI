using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Location { get; set; }

        [MaxLength(1000)]
        public string ExperienceRequired { get; set; }

        [MaxLength(500)]
        public string Skills { get; set; }

        [MaxLength(500)]
        public string SalaryRange { get; set; }

        [Required]
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiryDate { get; set; }

        [MaxLength(50)]
        public string JobType { get; set; } 

        public bool IsActive { get; set; } = true;
        public ICollection<Payment> Payments { get; set; }

    
    }
}
