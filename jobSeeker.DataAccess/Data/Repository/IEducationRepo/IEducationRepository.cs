using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IEducationRepo
{
    public interface IEducationRepository
    {
        Task AddAsync(Education education);
        Task<Education> GetByIdAsync(int id);
        Task<IEnumerable<Education>> GetByUserIdAsync(string userId);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
