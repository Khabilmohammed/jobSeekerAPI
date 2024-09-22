using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class PostImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostImageId { get; set; } // Primary key

        [Required]
        public string ImageUrl { get; set; } // URL of the image

        public int PostId { get; set; } // Foreign key to Post

        public Post Post { get; set; }
    }
}
