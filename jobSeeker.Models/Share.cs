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

        [Required(ErrorMessage = "SenderId is required.")]
        public string SenderId { get; set; }

        [Required(ErrorMessage = "RecipientId is required.")]
        public string RecipientId { get; set; }
        public DateTime SharedAt { get; set; } = DateTime.Now; 
        public Post Post { get; set; } // Navigation property
        public ApplicationUser Sender { get; set; } // Navigation property
        public ApplicationUser Recipient { get; set; }
    }

}

