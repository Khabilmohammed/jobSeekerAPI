﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models.DTO
{
    public class CertificateDto
    {
        public int CertificateId { get; set; }
        public string Name { get; set; }
        public string Issuer { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string ImageUrl { get; set; } 
        public string UserId { get; set; }
    }
}
