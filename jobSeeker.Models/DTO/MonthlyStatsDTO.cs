using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class MonthlyStatsDTO
    {
        public List<string> Months { get; set; }
        public List<int> JobPosts { get; set; }
        public List<int> Applications { get; set; }
        public List<int> UserRegistrations { get; set; }
        public List<int> CompanyRegistrations { get; set; }
    }
}
