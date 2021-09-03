using System.Threading.Tasks;
using DevnotMentor.WebAPI.ActionFilters;
using DevnotMentor.WebAPI.Helpers.Extensions;
using DevnotMentor.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DevnotMentor.Common.Requests.Mentorship;

namespace DevnotMentor.WebAPI.Controllers
{
    [ServiceFilter(typeof(TokenAuthentication))]
    [ValidateModelState]
    public class MentorshipController : BaseController
    {
        private readonly IMentorshipService _mentorshipService;

        public MentorshipController(IMentorshipService mentorshipService)
        {
            _mentorshipService = mentorshipService;
        }
        
        [HttpPost("/users/me/mentorships/{id}/finish")]
        public async Task<IActionResult> FinishAsync([FromRoute] int id)
        {
            var authenticatedUserId = User.GetId();
            var result = await _mentorshipService.FinishContinuingMentorshipAsync(authenticatedUserId, id);
            return ApiResponse(result);
        }

        [HttpPost("/users/me/mentorships/{id}/feedback")]
        public async Task<IActionResult> FeedbackAsync([FromRoute] int id, [FromBody] MentorshipFeedbackRequest MentorshipFeedbackRequest)
        {
            var authenticatedUserId = User.GetId();
            var result = await _mentorshipService.GiveFeedbackToFinishedMentorshipAsync(authenticatedUserId, id, MentorshipFeedbackRequest);
            return ApiResponse(result);
        }
    }
}