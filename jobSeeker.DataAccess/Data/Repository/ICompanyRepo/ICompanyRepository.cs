using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.ICompanyRepo
{
    public interface ICompanyRepository
    {
        Task<Company> CreateAsync(Company company);
        Task<Company> GetByIdAsync(int companyId);
        Task<bool> UpdateAsync(Company company);
        Task<bool> DeleteAsync(int companyId);
        Task<Company> GetByUserIdAsync(string userId);
        Task AddAsync(Company company);
    }
}
