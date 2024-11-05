using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.CertificateRepo
{
    public interface ICertificateRepository
    {
        Task<Certificate> AddAsync(Certificate certificate);
        Task<Certificate> GetByIdAsync(int id);
        Task<List<Certificate>> GetByUserIdAsync(string userId);
        Task DeleteAsync(int id);
    }
}
