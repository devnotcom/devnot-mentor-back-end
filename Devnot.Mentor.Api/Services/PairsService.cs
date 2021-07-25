using System.Threading.Tasks;
using AutoMapper;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.Configuration.Context;
using DevnotMentor.Api.Enums;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Repositories.Interfaces;
using DevnotMentor.Api.Services.Interfaces;

namespace DevnotMentor.Api.Services
{
    public class PairsService : BaseService, IPairsService
    {
        private readonly IMentorMenteePairsRepository pairsRepository;

        public PairsService(IMapper mapper,
                            ILoggerRepository logger,
                            IDevnotConfigurationContext devnotConfigurationContext, IMentorMenteePairsRepository pairsRepository) : base(mapper, logger, devnotConfigurationContext)
        {
            this.pairsRepository = pairsRepository;
        }

        public async Task<ApiResponse> Finish(int authorizedUserId, int pairsId)
        {
            var pair = await pairsRepository.GetByIdIncludeMenteeMentorAsync(pairsId);
            
            if (pair == null)
            {
                return new ErrorApiResponse(ResultMessage.NotFoundMentorMenteePair);
            }

            var authorizedUserRelatedToPair = pair.Mentee.UserId == authorizedUserId || pair.Mentor.UserId == authorizedUserId;

            if (authorizedUserRelatedToPair)
            {
                pair.State = MentorMenteePairStatus.Finished.ToInt();
                pairsRepository.Update(pair);
                return new SuccessApiResponse();
            }

            return new ErrorApiResponse(ResultMessage.Forbidden);
        }
    }
}