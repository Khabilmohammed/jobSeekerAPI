using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IExperienceService
{
    public interface IExperienceServices
    {
        Task<List<ExperienceDto>> GetAllExperiencesAsync(string userId);
        Task<ExperienceDto> GetExperienceByIdAsync(int id);
        Task<ExperienceDto> CreateExperienceAsync(CreateExperienceDto createExperienceDto, string userId);
        Task<ExperienceDto> UpdateExperienceAsync(int id, CreateExperienceDto createExperienceDto);
        Task<bool> DeleteExperienceAsync(int id);
        Task<bool> CheckUserExistsAsync(string userId);
    }
}
