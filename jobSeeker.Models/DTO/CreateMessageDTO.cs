using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class CreateMessageDTO
    {
        public string RecipientUserName { get; set; } 
        public string Content { get; set; }
    }
}
