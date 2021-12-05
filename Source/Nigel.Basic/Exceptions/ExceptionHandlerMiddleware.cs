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
            var errorCode = "error";
            var statusCode = HttpStatusCode.BadRequest;
            var exceptionType = exception.GetType();
            switch (exception)
            {
                case Exception e when exceptionType == typeof(UnauthorizedAccessException):
                    statusCode = HttpStatusCode.Unauthorized;
                    //记录异常日志
                    log = Log.ForContext<ExceptionHandlerMiddleware>();
                    log.Error($"Code={statusCode},message={e.Message}");
                    break;

                case AppException e when exceptionType == typeof(AppException):
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = e.Message;
                    log = Log.ForContext<AppException>();
                    log.Error($"Code={statusCode},message={e.Message}");
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    errorCode = "Internal Server Error";
                    log = Log.ForContext<Exception>();
                    log.Error(exception, exception.Message);
                    break;
            }

            // var response = new { code = statusCode, message = errorCode };
            var response = ResponseData.SetResult(errorCode, statusCode, errorCode);
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            ////记录异常日志
            //ILog log = LogManager.GetLogger(typeof(PopException));
            //log.Error(payload);

            return context.Response.WriteAsync(payload);
        }
    }
}