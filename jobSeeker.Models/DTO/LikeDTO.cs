﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class LikeDTO
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int LikeId { get; set; }
    }
}
