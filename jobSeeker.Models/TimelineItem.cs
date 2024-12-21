using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class TimelineItem
    {
        public string Type { get; set; } // "Message" or "SharedPost"
        public object Data { get; set; } // Can hold either MessageDTO or SharedPostDTO
        public DateTime Timestamp { get; set; }
    }
}
