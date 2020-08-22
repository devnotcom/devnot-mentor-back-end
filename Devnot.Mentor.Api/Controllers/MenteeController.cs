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
    public class MenteeController : BaseController
    {
        private IMenteeService menteeService;

        public MenteeController(IMenteeService menteeService)
        {
            this.menteeService = menteeService;
        }

        [HttpGet]
        public IActionResult Get(string userName)
        {
            var result = menteeService.GetMenteeProfile(userName);

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
        public async Task<IActionResult> Post([FromBody] MenteeProfileModel model)
        {
            var result = await menteeService.CreateMenteeProfile(model);

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
        [ServiceFilter(typeof(TokenAuthentication))]
        [Route("~/mentees/{menteeId}/mentors")]
        public async Task<IActionResult> ApplyToMentor(ApplyMentorModel model)
        {
            var checkResult = await menteeService.ApplyToMentor(model);

            if (checkResult.Success)
            {
                return Success(checkResult.Message);
            }

            return Error<object>(checkResult.Message, null);
        }

    }
}
