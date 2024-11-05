using AutoMapper;
using jobSeeker.DataAccess.Data.Repository.CertificateRepo;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.CertificateService
{
    public class CertificateServices:ICertificateServices
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IMapper _mapper;

        public CertificateServices(ICertificateRepository certificateRepository, IMapper mapper)
        {
            _certificateRepository = certificateRepository;
            _mapper = mapper;
        }

        public async Task<CertificateDto> CreateCertificateAsync(CreateCertificateDto createCertificateDto, string userId)
        {
            var certificate = _mapper.Map<Certificate>(createCertificateDto);
            certificate.UserId = userId;

            var createdCertificate = await _certificateRepository.AddAsync(certificate);
            return _mapper.Map<CertificateDto>(createdCertificate);
        }

        public async Task<CertificateDto> GetCertificateByIdAsync(int id)
        {
            var certificate = await _certificateRepository.GetByIdAsync(id);
            return _mapper.Map<CertificateDto>(certificate);
        }

        public async Task<List<CertificateDto>> GetCertificatesByUserIdAsync(string userId)
        {
            var certificates = await _certificateRepository.GetByUserIdAsync(userId);
            return _mapper.Map<List<CertificateDto>>(certificates);
        }

        public async Task DeleteCertificateAsync(int id)
        {
            await _certificateRepository.DeleteAsync(id);
        }

    }
}
