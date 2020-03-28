using Microsoft.AspNetCore.Builder;

namespace Nigel.Basic.Exceptions
{
    /// <summary>
    /// Class ExceptionExtension.
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Uses the custom exception handler.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>IApplicationBuilder.</returns>
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware(typeof(ExceptionHandlerMiddleware));
        }
    }
}