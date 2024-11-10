using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class UpdateCompanyDTO
    {
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string Website { get; set; }

        [StringLength(50, ErrorMessage = "Industry cannot exceed 50 characters.")]
        public string Industry { get; set; }

        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string Location { get; set; }

        [Range(1800, 2100, ErrorMessage = "Founded Year must be between 1800 and 2100.")]
        public int? FoundedYear { get; set; }

        [StringLength(50, ErrorMessage = "Size cannot exceed 50 characters.")]
        public string Size { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL for the Logo.")]
        public string LogoUrl { get; set; }
        public string about { get; set; }
    }
}
