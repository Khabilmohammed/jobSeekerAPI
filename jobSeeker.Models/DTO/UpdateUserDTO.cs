using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class UpdateUserDTO
    {

        [Required]
        public string UserId { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [RegularExpression(@"^\d{5,6}$", ErrorMessage = "Pincode must be a 5 or 6-digit number.")]
        public string Pincode { get; set; }

        public string ProfilePicture { get; set; }
        public IFormFile ProfilePictureFile { get; set; }
    }
}
