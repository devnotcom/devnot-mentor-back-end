﻿using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DevnotMentor.Api.CustomEntities.Request.MentorRequest;
using DevnotMentor.Api.Helpers.Extensions;
using Microsoft.AspNetCore.Http;

namespace DevnotMentor.Api.Controllers
{
    [ValidateModelState]
    [ApiController]
    public class MentorController : BaseController
    {
        private readonly IMentorService mentorService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public MentorController(IMentorService service, IHttpContextAccessor httpContextAccessor)
        {
            mentorService = service;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("/mentors/{userName}")]
        public async Task<IActionResult> GetAsync([FromRoute] string userName)
        {
            var result = await mentorService.GetMentorProfile(userName);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("/mentors/{userName}/mentees")]
        public async Task<IActionResult> GetMentees([FromRoute] string userName)
        {
            var result = await mentorService.GetMentees(userName);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("/mentors/{userName}/applications")]
        public async Task<IActionResult> GetApplications([FromRoute] string userName)
        {
            var result = await mentorService.GetApplicationsNotIncludeApproveds(userName);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("/mentors")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> Post([FromBody] CreateMentorProfileRequest request)
        {
            request.UserId = httpContextAccessor.HttpContext.User.Claims.GetUserId();

            var result = await mentorService.CreateMentorProfile(request);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("/mentors/{mentorId}/mentees/{menteeId}/accept")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> AcceptMentee([FromRoute] int mentorId, [FromRoute] int menteeId)
        {
            var mentorUserId = httpContextAccessor.HttpContext.User.Claims.GetUserId();

            var result = await mentorService.AcceptMentee(mentorUserId, mentorId, menteeId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("/mentors/{mentorId}/mentees/{menteeId}/reject")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> RejectMentee([FromRoute] int mentorId, [FromRoute] int menteeId)
        {
            var mentorUserId = httpContextAccessor.HttpContext.User.Claims.GetUserId();

            var result = await mentorService.RejectMentee(mentorUserId, mentorId, menteeId);

            return result.Success ? Success(result) : BadRequest(result);
        }
    }
}
