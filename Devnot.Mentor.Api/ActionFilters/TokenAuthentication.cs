using DevnotMentor.Api.Utilities.Security.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Common.Response;

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
            var tokenWithBearerKeyword = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (String.IsNullOrEmpty(tokenWithBearerKeyword))
            {
                context.Result = new UnauthorizedObjectResult(new ErrorApiResponse(ResultMessage.TokenCanNotBeEmptyOrNull));
                return;
            }

            if (!tokenWithBearerKeyword.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedObjectResult(new ErrorApiResponse(ResultMessage.TokenMustStartWithBearerKeyword));
                return;
            }

            try
            {
                var tokenWithoutBearerKeyword = tokenWithBearerKeyword.Split("Bearer ")[1];

                var resolveTokenResult = _tokenService.ResolveToken(tokenWithoutBearerKeyword);

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
                context.Result = new UnauthorizedObjectResult(new ErrorApiResponse(ResultMessage.InvalidToken));
            }
        }
    }
}
