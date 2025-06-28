using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class UserProfileDTO
    {
        
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Pincode { get; set; }
            public string ProfilePicture { get; set; }
            public string UserName { get; set; }

            public int FollowersCount { get; set; }
            public int FollowingCount { get; set; }
            public int PostCount { get; set; }
        

    }
}
