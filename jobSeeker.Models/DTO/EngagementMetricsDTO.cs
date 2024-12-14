using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class EngagementMetricsDTO
    {
        public List<string> Months { get; set; }
        public List<int> PostsCreated { get; set; }
        public List<int> LikesReceived { get; set; }
    }
}
