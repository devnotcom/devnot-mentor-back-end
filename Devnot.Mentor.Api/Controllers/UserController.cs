using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Helpers;
using DevnotMentor.Api.Helpers.Extensions;
using DevnotMentor.Api.Models;
using DevnotMentor.Api.Services;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DevnotMentor.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private IUserService _userService;
        private IHttpContextAccessor _httpContextAccessor;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetLogs()
        {
            try
            {
                var context = new MentorDBContext();
                var logs = context.Log.OrderByDescending(l => l.InsertDate).Take(100).ToList();
                var logList = new List<string>();

                foreach (var log in logs)
                {
                    logList.Add(log.InsertDate.ToString() + " - " + log.Message);
                }

                return Success(logList);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                    message += "\nInnerException: " + ex.InnerException.Message;

                return Success("Veritabanına bağlantı kurulamadı. Hata mesajı: " + message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _userService.Login(model);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Success(result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromForm] UserModel model)
        {
            var result = await _userService.Register(model);

            if (result.Success)
            {
                return Success(result);
            }
            else
            {
                return Error(result);
            }
        }

        protected void RemindPassword()
        {

        }

        [HttpPost]
        [Route("/users/{userId}/change-password")]
        public IActionResult ChangePassword([FromBody] PasswordUpdateModel model)
        {
            model.UserId = _httpContextAccessor.HttpContext.User.Claims.GetUserId();

            var checkResult = _userService.ChangePassword(model);

            return Ok(":D");
        }

        protected void UpdateUser()
        {

        }
    }
}