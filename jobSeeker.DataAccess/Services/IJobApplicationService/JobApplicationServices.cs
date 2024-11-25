using AutoMapper;
using jobSeeker.DataAccess.Data.Repository.IJobApplicationRepo;
using jobSeeker.Models.DTO;
using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jobSeeker.DataAccess.Services.CloudinaryService;

namespace jobSeeker.DataAccess.Services.IJobApplicationService
{
    public class JobApplicationServices:IJobApplicationServices
    {
        private readonly IJobApplicationRepository _repository;
        private readonly IMapper _mapper;
        private readonly CloudinaryServices _cloudinaryServices;
        public JobApplicationServices(IJobApplicationRepository repository, IMapper mapper, CloudinaryServices cloudinaryServices)
        {
            _repository = repository;
            _mapper = mapper;
            _cloudinaryServices = cloudinaryServices;
        }

        public async Task<JobApplication> ApplyForJobAsync(CreateJobApplicationDTO jobApplicationDto)
        {
            var resumeUrl = await _cloudinaryServices.UploadFileAsync(jobApplicationDto.ResumeFile, "resumes");
            // Map DTO to JobApplication model
            var jobApplication = _mapper.Map<JobApplication>(jobApplicationDto);
            jobApplication.ResumeUrl = resumeUrl;
            jobApplication.ApplicationDate = DateTime.UtcNow;
            jobApplication.Status = "Applied";

            return await _repository.CreateJobApplicationAsync(jobApplication);
        }

        public async Task<JobApplication> GetApplicationByIdAsync(int jobApplicationId)
        {
            return await _repository.GetJobApplicationByIdAsync(jobApplicationId);
        }

        public async Task<IEnumerable<JobApplication>> GetApplicationsForJobPostingAsync(int jobPostingId)
        {
            return await _repository.GetAllApplicationsForJobPostingAsync(jobPostingId);
        }

        public async Task<IEnumerable<JobApplication>> GetUserApplicationsAsync(string userId)
        {
            return await _repository.GetUserApplicationsAsync(userId);
        }

        public async Task<bool> ChangeApplicationStatusAsync(int jobApplicationId, string status)
        {
            return await _repository.UpdateJobApplicationStatusAsync(jobApplicationId, status);
        }

        public async Task<bool> RemoveApplicationAsync(int jobApplicationId)
        {
            return await _repository.DeleteJobApplicationAsync(jobApplicationId);
        }
    }
}
