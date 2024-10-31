using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class Experience
    {
        public int ExperienceId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
        public bool IsCurrent { get; set; }

        // Navigation property
        public ApplicationUser User { get; set; }
    }
}
