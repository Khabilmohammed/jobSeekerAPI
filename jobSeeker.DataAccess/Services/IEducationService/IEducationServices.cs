using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IEducationService
{
    public interface IEducationServices
    {
        Task<Education> AddEducationAsync(CreateEducationDTO createEducationDTO);
        Task<IEnumerable<EducationResponseDTO>> GetEducationsByUserIdAsync(string userId);
        Task<Education> GetEducationByIdAsync(int id);
        Task<Education> UpdateEducationAsync(int id, EducationResponseDTO createEducationDTO);
        Task<bool> DeleteEducationAsync(int id);
        
    }
}
