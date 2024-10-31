using AutoMapper;
using jobSeeker.DataAccess.Data.Repository.IExpericeRepo;
using jobSeeker.Models.DTO;
using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IExperienceService
{
    public class ExperienceService:IExperienceServices
    {
        private readonly IExperienceRepository _experienceRepository;
        private readonly IMapper _mapper;

        public ExperienceService(IExperienceRepository experienceRepository, IMapper mapper)
        {
            _experienceRepository = experienceRepository;
            _mapper = mapper;
        }
        public async Task<List<ExperienceDto>> GetAllExperiencesAsync(string userId)
        {
            var experiences = await _experienceRepository.GetAllExperiencesAsync(userId);
            return _mapper.Map<List<ExperienceDto>>(experiences);
        }

        public async Task<ExperienceDto> GetExperienceByIdAsync(int id)
        {
            var experience = await _experienceRepository.GetExperienceByIdAsync(id);
            return _mapper.Map<ExperienceDto>(experience);
        }

        public async Task<ExperienceDto> CreateExperienceAsync(CreateExperienceDto createExperienceDto, string userId)
        {
            var experience = _mapper.Map<Experience>(createExperienceDto);
            experience.UserId = userId;
            await _experienceRepository.CreateExperienceAsync(experience);
            return _mapper.Map<ExperienceDto>(experience);
        }

        public async Task<ExperienceDto> UpdateExperienceAsync(int id, CreateExperienceDto createExperienceDto)
        {
            var experience = _mapper.Map<Experience>(createExperienceDto);
            experience.ExperienceId = id; // Ensure the correct ID is set
            await _experienceRepository.UpdateExperienceAsync(experience);
            return _mapper.Map<ExperienceDto>(experience);
        }

        public async Task<bool> DeleteExperienceAsync(int id)
        {
            return await _experienceRepository.DeleteExperienceAsync(id);
        }
        public async Task<bool> CheckUserExistsAsync(string userId)
        {
            return await _experienceRepository.CheckUserExistsAsync(userId);
        }

    }
}
