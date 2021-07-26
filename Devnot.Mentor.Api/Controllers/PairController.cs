using System.Threading.Tasks;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.CustomEntities.Request.PairRequest;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    [ServiceFilter(typeof(TokenAuthentication))]
    [ValidateModelState]
    [ApiController]
    [Route("pairs")]
    public class PairController : BaseController
    {
        private readonly IPairService pairsService;

        public PairController(IPairService pairsService)
        {
            this.pairsService = pairsService;
        }

        [HttpPost("{id}/finish")]
        public async Task<IActionResult> Finish(int id)
        {
            var authorizedUserId = User.Claims.GetUserId();
            var result = await pairsService.FinishByIdAndAuthorizedUser(authorizedUserId, id);

            return result.Success ? NoContent() : BadRequest(result);
        }

        [HttpPost("{id}/feedback")]
        public async Task<IActionResult> Feedback(int id, [FromBody] PairFeedbackRequest pairFeedbackRequest)
        {
            var authorizedUserId = User.Claims.GetUserId();
            var result = await pairsService.FeedbackByIdAndAuthorizedUser(authorizedUserId, id, pairFeedbackRequest);

            return result.Success ? NoContent() : BadRequest(result);
        }
    }
}