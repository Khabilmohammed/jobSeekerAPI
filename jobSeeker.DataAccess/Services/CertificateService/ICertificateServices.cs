using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.CertificateService
{
    public interface ICertificateServices
    {
        Task<CertificateDto> CreateCertificateAsync(CreateCertificateDto createCertificateDto, string userId);
        Task<CertificateDto> GetCertificateByIdAsync(int id);
        Task<List<CertificateDto>> GetCertificatesByUserIdAsync(string userId);
        Task DeleteCertificateAsync(int id);
    }
}
