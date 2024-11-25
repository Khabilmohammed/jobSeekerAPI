using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class CreateJobApplicationDTO
    {
        public int JobPostingId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public decimal ExpectedSalary { get; set; }
        [Required]
        public IFormFile ResumeFile { get; set; } 
        public string CoverLetter { get; set; }
    }
}
