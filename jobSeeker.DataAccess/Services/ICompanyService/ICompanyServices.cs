using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.ICompanyService
{
    public interface ICompanyServices
    {
        Task<CompanyDTO> CreateCompanyAsync(CreateCompanyDTO createCompanyDTO);
        Task<CompanyDTO> GetCompanyByIdAsync(int companyId);
        Task<bool> UpdateCompanyAsync(int companyId, UpdateCompanyDTO updateCompanyDTO);
        Task<bool> DeleteCompanyAsync(int companyId);
        Task<CompanyDTO> GetCompanyByUserIdAsync(string userId);
        Task AddAsync(Company company);
    }
}
