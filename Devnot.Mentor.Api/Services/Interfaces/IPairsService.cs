using System.Threading.Tasks;
using DevnotMentor.Api.Common.Response;

namespace DevnotMentor.Api.Services.Interfaces
{
    public interface IPairsService
    {
        Task<ApiResponse> Finish(int userId, int pairsId);
    }
}