using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class ChangeUserRoleRequestDTO
    {
        public string UserId { get; set; }
        public string NewRole { get; set; }
    }
}
