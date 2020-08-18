using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Common;
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
        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
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

        [HttpPost]
        [Route("/users/{userId}/change-password")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordUpdateModel model)
        {
            model.UserId = _httpContextAccessor.HttpContext.User.Claims.GetUserId();

            var checkResult = await _userService.ChangePassword(model);

            if (checkResult.Success)
            {
                return Success<bool>(checkResult);
            }

            return Error<bool>(checkResult);
        }

        [HttpPost]
        [Route("/users/{userId}/update-profile")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> UpdateUser([FromForm] UserUpdateModel model)
        {
            model.UserId = _httpContextAccessor.HttpContext.User.Claims.GetUserId();

            var checkResult = await _userService.Update(model);

            if (checkResult.Success)
            {
                return Success(checkResult);
            }
            return Error(checkResult);
        }

        [Route("/user/remind-password")]
        public async Task<IActionResult> RemindPassword(string email)
        {
            var result = await _userService.RemindPassword(email);

            if (result.Success)
            {
                return Success<string>("Mail başarıyla gönderildi..");
            }
            return Error<bool>(result.Message, false);
        }

        [HttpPost]
        [Route("/user/remind-password-complete")]
        public async Task<IActionResult> RemindPasswordCompleteAsync(RemindPasswordCompleteModel model)
        {
            var result = await _userService.RemindPasswordComplete(model);

            if (result.Success)
            {
                return Success<string>(result.Message);
            }

            return Error<string>(result.Message, null);
        }
    }
}