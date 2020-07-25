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
        IMenteeService service;
        public MenteeController(IMenteeService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string userName)
        {
            var result = await service.GetMenteeProfile(userName);

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
        public async Task<IActionResult> Post([FromBody]MenteeProfileModel model)
        {
            var result = await service.CreateMenteeProfile(model);

            if (result.Success)
            {
                return Success(result);
            }
            else
            {
                return Error(result);
            }
        }
    }
}
