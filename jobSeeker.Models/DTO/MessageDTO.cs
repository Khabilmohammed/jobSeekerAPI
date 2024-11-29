using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        public string SenderId { get; set; }
        public string SenderUserName { get; set; }
        public string RecipientId { get; set; }
        public string RecipientUserName { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime SentAt { get; set; }

    }
}
