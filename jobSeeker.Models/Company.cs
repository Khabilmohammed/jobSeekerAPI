using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class Company
    {
        public int CompanyId { get; set; } 
       
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }  
        public string about { get; set; }
        public string Website { get; set; }

        public string Industry { get; set; }

        [StringLength(100, ErrorMessage = "Location cannot be longer than 100 characters.")]
        public string Location { get; set; }
        public int? FoundedYear { get; set; }
        public string Size { get; set; }
        public string LogoUrl { get; set; } 
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsCompleted { get; set; } = false;

    }
}
