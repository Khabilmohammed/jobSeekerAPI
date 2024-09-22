using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class CreatePostDTO
    {
        public string UserId { get; set; }
        public string Content { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
