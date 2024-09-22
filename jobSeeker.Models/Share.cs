using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class Share
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShareId { get; set; } // Primary key

        [Required(ErrorMessage = "PostId is required.")]
        public int PostId { get; set; } // Foreign key to Post

        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; } // Foreign key to User model
        public DateTime SharedAt { get; set; } = DateTime.Now; // Automatically set the share timestamp

        public Post Post { get; set; } // Navigation property
        public ApplicationUser User { get; set; } // Navigation property
    }

}

