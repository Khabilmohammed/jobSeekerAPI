using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class SavedPost
    {
        public int SavedPostId { get; set; } // Primary key
        public string UserId { get; set; } // Foreign key to User
        public int PostId { get; set; } // Foreign key to Post

        public virtual ApplicationUser User { get; set; } // Navigation property to User
        public virtual Post Post { get; set; } // Navigation property to Post
    }
}
