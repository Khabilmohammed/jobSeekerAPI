
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace jobSeeker.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }


        [StringLength(50, ErrorMessage = "Middle Name cannot be longer than 50 characters.")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [StringLength(100, ErrorMessage = "City cannot be longer than 100 characters.")]
        public string City { get; set; }

        [StringLength(100, ErrorMessage = "Country cannot be longer than 100 characters.")]
        public string Country { get; set; }

        [RegularExpression(@"^\d{5,6}$", ErrorMessage = "Pincode must be a 5 or 6-digit number.")]
        public string Pincode { get; set; }


        [Url(ErrorMessage = "Please enter a valid URL for the Profile Picture.")]
        public string ProfilePicture { get; set; }


        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain letters and numbers.")]
        public override string UserName { get; set; }

        [NotMapped]  // This prevents this property from being added to the database
        public string Role { get; set; }

        public List<Experience> Experiences { get; set; } = new List<Experience>();
        public List<Certificate> Certificates { get; set; } = new List<Certificate>();
        public List<Education> Educations { get; set; }= new List<Education>();

        public Company Company { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }

        public ICollection<Share> SharesSent { get; set; }
        public ICollection<Share> SharesReceived { get; set; }
    }
}
