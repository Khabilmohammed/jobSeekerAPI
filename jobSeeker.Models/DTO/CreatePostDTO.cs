using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class CreatePostDTO
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(2000, ErrorMessage = "Content cannot be longer than 2000 characters.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}
