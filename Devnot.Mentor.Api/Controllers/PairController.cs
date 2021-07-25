using System.Threading.Tasks;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevnotMentor.Api.Controllers
{
    [ValidateModelState]
    [ApiController]
    [Route("pairs")]
    public class PairController : BaseController
    {
        private readonly IPairsService pairsService;

        public PairController(IPairsService pairsService)
        {
            this.pairsService = pairsService;
        }

        [HttpPost("{id}/finish")]
        public async Task<IActionResult> Finish(int id)
        {
            var authorizedUserId = User.Claims.GetUserId();
            var result = await pairsService.Finish(authorizedUserId, id);

            return result.Success ? NoContent() : result.Message == ResultMessage.Forbidden ? Forbidden(result) : BadRequest(result);
        }

        [HttpPost("{id}/mentee/comment")]
        public async Task<IActionResult> MenteeComment(int id)
        {
            return null;
        }

        [HttpPost("{id}/mentor/comment")]
        public async Task<IActionResult> MentorComment(int id)
        {
            return null;
        }
    }
}