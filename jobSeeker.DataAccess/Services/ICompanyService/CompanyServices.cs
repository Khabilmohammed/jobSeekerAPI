using AutoMapper;
using jobSeeker.DataAccess.Data.Repository.ICompanyRepo;
using jobSeeker.Models.DTO;
using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using jobSeeker.DataAccess.Services.CloudinaryService;
using Microsoft.AspNetCore.Mvc;

namespace jobSeeker.DataAccess.Services.ICompanyService
{
    public class CompanyServices:ICompanyServices
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly CloudinaryServices _cloudinaryServices;    
        public CompanyServices(ICompanyRepository companyRepository, IMapper mapper, CloudinaryServices cloudinaryServices)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _cloudinaryServices = cloudinaryServices;
        }
        public async Task<CompanyDTO> CreateCompanyAsync(CreateCompanyDTO createCompanyDTO)
        {
            try
            {
                var company = _mapper.Map<Company>(createCompanyDTO);
                company.CreatedAt = DateTime.UtcNow;
                company.UpdatedAt = DateTime.UtcNow;

                var createdCompany = await _companyRepository.CreateAsync(company);
                return _mapper.Map<CompanyDTO>(createdCompany);
            }
            catch (Exception ex)
            {
                // Log the exception (you can log it to a file, or console, or a logging service)
                Console.WriteLine($"Error creating company: {ex.Message}");
                throw; // Rethrow or handle it as needed
            }
        }
        public async Task<CompanyDTO> GetCompanyByIdAsync(int companyId)
        {
            var company = await _companyRepository.GetByIdAsync(companyId);
            return company != null ? _mapper.Map<CompanyDTO>(company) : null;
        }

        public async Task<bool> UpdateCompanyAsync(int companyId,UpdateCompanyDTO updateCompanyDTO)
        {
            var company = await _companyRepository.GetByIdAsync(companyId);
            if (company == null) return false;

            // Handle Logo upload if provided
            if (updateCompanyDTO.LogoUrl != null)
            {
                var logoUploadResult = await _cloudinaryServices.UploadImageAsync(updateCompanyDTO.LogoUrl);
                if (logoUploadResult.Error != null)
                {
                    throw new InvalidOperationException("Failed to upload the logo.");
                }
                company.LogoUrl = logoUploadResult.Url.ToString();
            }

            // Handle Banner upload if provided
            if (updateCompanyDTO.BannerImage != null)
            {
                var bannerUploadResult = await _cloudinaryServices.UploadImageAsync(updateCompanyDTO.BannerImage);
                if (bannerUploadResult.Error != null)
                {
                    throw new InvalidOperationException("Failed to upload the banner.");
                }
                company.Banner = bannerUploadResult.Url.ToString();
            }

            _mapper.Map(updateCompanyDTO, company);
            company.UpdatedAt = DateTime.UtcNow;
            return await _companyRepository.UpdateAsync(company);
        }

        public async Task<bool> DeleteCompanyAsync(int companyId)
        {
            return await _companyRepository.DeleteAsync(companyId);
        }

        public async Task<CompanyDTO> GetCompanyByUserIdAsync(string userId)
        {
            var company = await _companyRepository.GetByUserIdAsync(userId);
            return company != null ? _mapper.Map<CompanyDTO>(company) : null;
        }
        public async Task AddAsync(Company company)
        {
             await _companyRepository.AddAsync(company);
        }
    }
}
