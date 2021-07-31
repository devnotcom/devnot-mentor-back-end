using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.CustomEntities.Dto;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;

namespace DevnotMentor.Api.Services
{
    public class ApplicationService : BaseService, IApplicationService
    {

        private readonly IMentorApplicationsRepository applicationsRepository;

        public ApplicationService(
            IMapper mapper,
            ILoggerRepository loggerRepository,
            IDevnotConfigurationContext devnotConfigurationContext,
            IMentorApplicationsRepository mentorApplicationsRepository
        ) : base(mapper, loggerRepository, devnotConfigurationContext)
        {
            this.applicationsRepository = mentorApplicationsRepository;
        }

        public async Task<ApiResponse<List<MentorApplicationsDto>>> GetApplicationsByUserIdAsync(int userId)
        {
            var applications = await applicationsRepository.GetApplicationsByUserIdAsync(userId);
            var applicationsDto = mapper.Map<List<MentorApplicationsDto>>(applications);

            return new SuccessApiResponse<List<MentorApplicationsDto>>(applicationsDto);
        }
    }
}