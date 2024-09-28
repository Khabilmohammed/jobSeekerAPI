using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class PostDTO
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }
        public ICollection<LikeDTO> Likes { get; set; }
        public ICollection<ShareDTO> Shares { get; set; }
        public ICollection<PostImageDTO> Images { get; set; }


        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
