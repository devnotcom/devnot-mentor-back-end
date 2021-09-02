using DevnotMentor.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DevnotMentor.Common.API;

namespace DevnotMentor.WebAPI.ActionFilters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ErrorApiResponse(ResultMessage.InvalidModel));
            }
        }
    }
}
