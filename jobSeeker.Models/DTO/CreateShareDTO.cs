using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class CreateShareDTO
    {
        public int PostId { get; set; }
        public string SenderId { get; set; }    // The ID of the user sharing the post
        public string RecipientId { get; set; } 
    }
}
