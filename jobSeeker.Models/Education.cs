using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class Education
    {
        public int EducationId { get; set; } 
        public string Degree { get; set; } 
        public string Institution { get; set; } 
        public string FieldOfStudy { get; set; } 
        public string StartDate { get; set; }
        public string EndDate { get; set; } 
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
