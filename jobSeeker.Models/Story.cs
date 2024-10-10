using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class Story
    {
        public int StoryId { get; set; }
        public string UserId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationTime { get; set; }

        // Backing field for IsActive
        private bool _isActive;

        // Property with a public setter
        public bool IsActive
        {
            get => _isActive; // Get current state
            set => _isActive = value; // Allow setting of state
        }

        public ApplicationUser User { get; set; }
    }

}
