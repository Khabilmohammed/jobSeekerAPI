using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public class Follow
    {
        [Key, Column(Order = 0)]
        public string FollowerId { get; set; }  // User who follows
        public ApplicationUser Follower { get; set; }

        [Key, Column(Order = 1)]
        public string FollowingId { get; set; }  // User who is being followed
        public ApplicationUser Following { get; set; }

        public DateTime FollowedAt { get; set; }  // Timestamp of when the user followed another user
    }
}
