using jobSeeker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.ICompanyRepo
{
    public class CompanyRepository:ICompanyRepository
    {
        public readonly ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
       
        public async Task<Company> CreateAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }
        public async Task<Company> GetByIdAsync(int companyId)
        {
            return await _context.Companies.FindAsync(companyId);
        }

        public async Task<bool> UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(int companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company == null) return false;

            _context.Companies.Remove(company);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Company> GetByUserIdAsync(string userId)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task AddAsync(Company company)
        {
            // Check if the company already exists (optional)
            if (await _context.Companies.AnyAsync(c => c.Name == company.Name && c.UserId == company.UserId))
            {
                throw new Exception("A company with the same name already exists.");
            }

            // Add the company to the database
            company.CreatedAt = DateTime.UtcNow;
            company.UpdatedAt = DateTime.UtcNow;
            _context.Companies.Add(company);

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }
    }
}
