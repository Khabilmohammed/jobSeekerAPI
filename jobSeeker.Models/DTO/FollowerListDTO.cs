using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class FollowerListDTO
    {
        public string UserId { get; set; }
        public List<followUserdetailDTO> Followers { get; set; }
        public List<followUserdetailDTO> Following { get; set; }
    }
}
