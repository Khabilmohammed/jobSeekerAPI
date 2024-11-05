using jobSeeker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.CertificateRepo
{
    public class CertificateRepository:ICertificateRepository
    {
        private readonly ApplicationDbContext _context;

        public CertificateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Certificate> AddAsync(Certificate certificate)
        {
            await _context.Certificates.AddAsync(certificate);
            await _context.SaveChangesAsync();
            return certificate;
        }

        public async Task<Certificate> GetByIdAsync(int id)
        {
            return await _context.Certificates
                .Include(c => c.User) 
                .FirstOrDefaultAsync(c => c.CertificateId == id);
        }

        public async Task<List<Certificate>> GetByUserIdAsync(string userId)
        {
            return await _context.Certificates
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate != null)
            {
                _context.Certificates.Remove(certificate);
                await _context.SaveChangesAsync();
            }
        }

    }
}
