using Devnot.Mentor.Api.Common;
using Devnot.Mentor.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Devnot.Mentor.Api.Infrastructure.ActionFilters
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


				context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
