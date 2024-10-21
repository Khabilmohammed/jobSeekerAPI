using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class LikeCreateDTO
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
    }
}
