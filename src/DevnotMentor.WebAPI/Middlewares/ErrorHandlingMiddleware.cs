using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DevnotMentor.Common;
using DevnotMentor.Common.API;
using DevnotMentor.Data.Interfaces;
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
        public async Task Invoke(HttpContext context, ILogRepository loggerRepository)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
#pragma warning disable 4014
                loggerRepository.WriteErrorAsync(error);
#pragma warning disable 4014

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

