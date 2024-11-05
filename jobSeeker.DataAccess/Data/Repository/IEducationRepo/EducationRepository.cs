using jobSeeker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IEducationRepo
{
    public class EducationRepository:IEducationRepository
    {
        private readonly ApplicationDbContext _context;

        public EducationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Education education)
        {
            await _context.Educations.AddAsync(education);
        }

        public async Task<Education> GetByIdAsync(int id)
        {
            return await _context.Educations.FindAsync(id);
        }

        public async Task<IEnumerable<Education>> GetByUserIdAsync(string userId)
        {
            return await _context.Educations.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var education = await GetByIdAsync(id);
            if (education != null)
            {
                _context.Educations.Remove(education);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
