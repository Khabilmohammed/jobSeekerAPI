using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; } // Primary key

        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }

        [StringLength(2000, ErrorMessage = "Content cannot be longer than 2000 characters.")]
        public string Content { get; set; } // Text content of the post
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<PostImage> Images { get; set; } = new List<PostImage>(); // Initialize the image list to avoid null issues
        public ICollection<Comment> Comments { get; set; } = new List<Comment>(); // Navigation property for comments                                                                                                           
        public ICollection<Like> Likes { get; set; } = new List<Like>(); // Navigation property for likes
        public ICollection<Share> Shares { get; set; } = new List<Share>();

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }

}
