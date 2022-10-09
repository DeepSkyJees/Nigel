using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nigel.Basic.Exceptions;

namespace Nigel.Extensions.AspNetCore
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
        /// The logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public ExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
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
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorMessage = ExceptionCode.SystemUnKnownError.ToString();
            int statusCode = (int)HttpStatusCode.BadRequest;
            var exceptionType = exception.GetType();
            var errorCode = nameof(ExceptionCode.SystemUnKnownError);
            switch (exception)
            {
                case TypeConvertException e when exceptionType == typeof(TypeConvertException):
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = $"{ExceptionCode.ConvertTypeError},{e.Message}";
                    //记录异常日志 
                    _logger.LogError($"TypeConvertException:StatueCode={statusCode},{e.ToString()}");
                    break;
                case ConfigException e when exceptionType == typeof(ConfigException):
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = $"{ExceptionCode.ConfigError},{e.Message}";
                    //记录异常日志 
                    _logger.LogError($"ConfigException:StatueCode={statusCode},{e.ToString()}");
                    break;
                case AppException e when exceptionType == typeof(AppException):
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError($"AppException:StatueCode={statusCode},{e.ToString()}");
                    break;

                default:
                    if (exception is AppException customException)
                    {
                        _logger.LogError($"AppException:StatueCode={statusCode},{customException.ToString()}");
                    }
                    else
                    {
                        statusCode = context.Response.StatusCode;
                        _logger.LogError($"UnknownException:StatueCode={statusCode},ErrorCode:{nameof(ExceptionCode.SystemUnKnownError)},ErrorMessage:{ExceptionCode.SystemUnKnownError},ExceptionMessage:{exception.Message}");
                    }


                    break;
            }

            // var response = new { code = statusCode, message = errorCode };
            var response = ApiResponseResult.GetErrorResponseResult(statusCode, errorCode, errorMessage);
            var payload = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(payload);
        }
    }
}