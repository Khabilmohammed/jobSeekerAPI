using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class CreateStoryDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
    }
}
