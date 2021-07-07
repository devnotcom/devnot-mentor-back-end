using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Common.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DevnotMentor.Api.Middlewares
{
	/// <summary>
    /// for all types of unexpected errors
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        /// <summary>
        /// error handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
               
                response.ContentType = "application/json";

                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var resp = new ErrorApiResponse(ResultMessage.InternalServerError);

                var result = JsonSerializer.Serialize(resp);

                logger.LogError(error, result); 

                await response.WriteAsync(result);
            }
        }
    }
}

