using AutoMapper;
using jobSeeker.DataAccess.Data.Repository.IEducationRepo;
using jobSeeker.DataAccess.Services.IUsermanagemetService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IEducationService
{
    public class EducationServices:IEducationServices
    {
        private readonly IEducationRepository _educationRepository;
        private readonly IMapper _mapper;
        private readonly IUserManagementService _userManagementService;

        public EducationServices(IEducationRepository educationRepository,
             IMapper mapper,
             IUserManagementService userManagementService)
        {
            _educationRepository = educationRepository;
            _mapper = mapper;
            _userManagementService = userManagementService;
        }

        public async Task<Education> AddEducationAsync(CreateEducationDTO createEducationDTO)
        {
            var education = _mapper.Map<Education>(createEducationDTO);
            await _educationRepository.AddAsync(education);
            await _educationRepository.SaveChangesAsync();
            return education; // Return the created entity
        }

        public async Task<IEnumerable<EducationResponseDTO>> GetEducationsByUserIdAsync(string userId)
        {
            var educations = await _educationRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<EducationResponseDTO>>(educations);
        }

        public async Task<Education> GetEducationByIdAsync(int id)
        {
            return await _educationRepository.GetByIdAsync(id);
        }

        public async Task<Education> UpdateEducationAsync(int id, EducationResponseDTO createEducationDTO)
        {
            var existingEducation = await _educationRepository.GetByIdAsync(id);
            if (existingEducation == null)
            {
                return null; // Indicate that the education record was not found
            }

            // Map updated fields from DTO to the existing entity
            _mapper.Map(createEducationDTO, existingEducation);

            await _educationRepository.SaveChangesAsync();
            return existingEducation;
        }

        public async Task<bool> DeleteEducationAsync(int id)
        {
            var education = await _educationRepository.GetByIdAsync(id);
            if (education != null)
            {
                await _educationRepository.DeleteAsync(id);
                await _educationRepository.SaveChangesAsync();
                return true; // Indicate successful deletion
            }
            return false; 
        }
        public async Task<bool> UserExistsAsync(string userId)
        {
            return await _userManagementService.UserExistsAsync(userId); 
        }

    }
}
