using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;
using DevnotMentor.Api.CustomEntities.Request.PairRequest;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IPairService
    {
        Task<ApiResponse> FinisForAuthorizedUserById(int authorizedUserId, int pairId);
        Task<ApiResponse> FeedbackForAuthorizedUserById(int authorizedUserId, int pairId, PairFeedbackRequest pairFeedbackRequest);
    }
}