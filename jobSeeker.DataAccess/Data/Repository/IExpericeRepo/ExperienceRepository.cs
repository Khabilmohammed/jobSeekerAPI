using jobSeeker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IExpericeRepo
{
    public class ExperienceRepository:IExperienceRepository
    {
        private readonly ApplicationDbContext _context;
        public ExperienceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Experience>> GetAllExperiencesAsync(string userId)
        {
            return await _context.Experiences
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<Experience> GetExperienceByIdAsync(int id)
        {
            return await _context.Experiences.FindAsync(id);
        }

        public async Task<Experience> CreateExperienceAsync(Experience experience)
        {
            await _context.Experiences.AddAsync(experience);
            await _context.SaveChangesAsync();
            return experience;
        }

        public async Task<Experience> UpdateExperienceAsync(Experience experience)
        {
            _context.Experiences.Update(experience);
            await _context.SaveChangesAsync();
            return experience;
        }

        public async Task<bool> DeleteExperienceAsync(int id)
        {
            var experience = await GetExperienceByIdAsync(id);
            if (experience == null) return false;

            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CheckUserExistsAsync(string userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

    }
}
