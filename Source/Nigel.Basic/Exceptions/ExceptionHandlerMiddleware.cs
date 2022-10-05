using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Nigel.Basic.Exceptions
{
    /// <summary>
    /// Class ExceptionHandlerMiddleware.
    /// </summary>
    internal class ExceptionHandlerMiddleware
    {
        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Task.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        /// <summary>
        /// Handles the exception asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>Task.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //记录异常日志
            ILogger log;
            var errorMessage = ExceptionCode.SystemUnKnownError.ToString();
            int statusCode = (int)HttpStatusCode.BadRequest;
            var exceptionType = exception.GetType();
            var errorCode = (int)ExceptionCode.SystemUnKnownError;
            switch (exception)
            { 

                case TypeConvertException e when exceptionType == typeof(TypeConvertException):
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = $"{ExceptionCode.UnauthorizedFailed.ToString()},{e.Message}";
                    //记录异常日志
                    log = Log.ForContext<TypeConvertException>();
                    log.Error($"TypeConvertException:Code={statusCode},message={e.Message}");
                    break;

                case BizException e when exceptionType == typeof(BizException):
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorMessage = $"{ExceptionCode.SystemUnKnownError.ToString()},{e.Message}";
                    log = Log.ForContext<BizException>();
                    log.Error($"BizException:Code={statusCode},message={e.Message}");
                    break;

                default:
                    statusCode = context.Response.StatusCode; 
                    log = Log.ForContext<Exception>();
                    log.Error(exception, exception.Message);
                    break;
            }

            // var response = new { code = statusCode, message = errorCode };
            var response = ApiResponseResult.GetErrorResponseResult(statusCode,errorCode,errorMessage);
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(payload);
        }
    }
}