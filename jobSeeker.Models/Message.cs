using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string SenderId { get; set; }
        public string SenderUserName { get; set; }
        
        [ForeignKey("SenderId")]
        public virtual ApplicationUser Sender { get; set; }
        public string RecipientId { get; set; }
        public string RecipientUserName { get; set; }
        
        [ForeignKey("RecipientId")]
        public virtual ApplicationUser Recipient { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? DateRead { get; set; } 

        public bool SenderDeleted {  get; set; }    
        public bool RecipientDeleted { get; set; }

    }
}
