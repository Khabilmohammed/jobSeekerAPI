using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class CompanyDTO
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public string Location { get; set; }
        public int? FoundedYear { get; set; }
        public string Size { get; set; }
        public string LogoUrl { get; set; }
        public string UserId { get; set; }
        public string about { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
