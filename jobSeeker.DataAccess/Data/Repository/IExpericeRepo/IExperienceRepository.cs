using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IExpericeRepo
{
    public interface IExperienceRepository
    {
        Task<List<Experience>> GetAllExperiencesAsync(string userId);
        Task<Experience> GetExperienceByIdAsync(int id);
        Task<Experience> CreateExperienceAsync(Experience experience);
        Task<Experience> UpdateExperienceAsync(Experience experience);
        Task<bool> DeleteExperienceAsync(int id);
        Task<bool> CheckUserExistsAsync(string userId);
    }
}
