using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; } // Primary key

        [Required(ErrorMessage = "PostId is required.")]
        public int PostId { get; set; } // Foreign key to Post

        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; } // Foreign key to User model

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(1000, ErrorMessage = "Content cannot be longer than 1000 characters.")]
        public string Content { get; set; } // Comment content
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Automatically set the creation time

        [ForeignKey("PostId")]
        public Post Post { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } // Navigation property
    }

}
