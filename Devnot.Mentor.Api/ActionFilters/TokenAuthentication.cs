using DevnotMentor.Api.Common;
using DevnotMentor.Api.Utilities.Security.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevnotMentor.Api.ActionFilters
{
    public class TokenAuthentication : ActionFilterAttribute
    {
        private ITokenService _tokenService;

        public TokenAuthentication(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (String.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedObjectResult(new ApiResponse
                {
                    Success = false,
                    Message = "Yetkisiz işlem! Lütfen Header içerisinde token bilgisi gönderdiğinizden emin olunuz."
                });

                return;
            }

            try
            {
                var resolveTokenResult = _tokenService.ResolveToken(token);

                if (!resolveTokenResult.IsValid)
                {
                    context.Result = new UnauthorizedObjectResult(new ApiResponse
                    {
                        Success = false,
                        Message = resolveTokenResult.ErrorMessage
                    });
                }

                var claimsIdentity = new ClaimsIdentity(resolveTokenResult.Claims);

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                context.HttpContext.User = claimsPrincipal;
            }
            catch (Exception ex)
            {
                context.Result = new UnauthorizedObjectResult(new ApiResponse
                {
                    Success = false,
                    Message = "Yetkisiz işlem! Lütfen geçerli bir token bilgisi giriniz."
                });
            }
        }
    }
}
