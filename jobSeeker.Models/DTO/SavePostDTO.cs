using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class SavePostDTO
    {
        public string UserId { get; set; } // Assuming you want to associate the saved post with a user
        public int PostId { get; set; } // The ID of the post to be saved
        public string UserName { get; set; } 
        public PostDTO Post { get; set; }
        public string PostContent { get; set; }
    }
}
