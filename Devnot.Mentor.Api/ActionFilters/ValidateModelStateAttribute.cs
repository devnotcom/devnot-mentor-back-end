using DevnotMentor.Api.Common;
using DevnotMentor.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;

namespace DevnotMentor.Api.ActionFilters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
				var responseMessages = context.HttpContext.RequestServices.GetService(typeof(IOptions<ResponseMessages>)) as IOptions<ResponseMessages>;

				context.Result = new BadRequestObjectResult(new ApiResponse
				{
					Success = false,
					Message = responseMessages.Value.Values["InvalidModel"]
				});

				base.OnActionExecuting(context);
            }
        }
    }
}
