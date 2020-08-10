using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Controllers
{
    [ValidateModelState]
    [Route("[controller]")]
    [ApiController]
    public class MentorController : BaseController
    {
        IMentorService mentorService;

        public MentorController(IMentorService service)
        {
            mentorService = service;
        }

        [HttpGet]
        public IActionResult Get(string userName)
        {
            var result = mentorService.GetMentorProfile(userName);

            if (result.Success)
            {
                return Success(result);
            }
            else
            {
                return Error(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MentorProfileModel model)
        {
            var result = await mentorService.CreateMentorProfile(model);

            if (result.Success)
            {
                return Success(result);
            }
            else
            {
                return Error(result);
            }
        }

        [HttpPost]
        [Route("/mentors/{mentorUserId}/mentees/{menteeUserId}/accept")]
        public async Task<IActionResult> AcceptMentee([FromRoute] int mentorUserId, [FromRoute] int menteeUserId)
        {
            var result = await mentorService.AcceptMentee(mentorUserId, menteeUserId);

            if (result.Success)
            {
                return Success(result);
            }

            return Error<object>(result.Message, null);
        }

        [HttpPost]
        [Route("/mentors/{mentorUserId}/mentees/{menteeUserId}/reject")]
        public async Task<IActionResult> RejectMentee([FromRoute] int mentorUserId, [FromRoute] int menteeUserId)
        {
            var result = await mentorService.RejectMentee(mentorUserId, menteeUserId);

            if (result.Success)
            {
                return Success(result);
            }

            return Error<object>(result.Message, null);
        }
    }
}
