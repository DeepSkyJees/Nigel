using System;
using System.Globalization;
using System.Linq;

namespace Nigel.Basic.Exceptions
{
    /// <summary>
    /// Class AppException.
    /// </summary>
    /// <seealso cref="System.Exception" />
    /// <seealso cref="Exception" />
    public class AppException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppException" /> class.
        /// </summary>
        public AppException()
        {
            ErrorCode = nameof(ExceptionCode.AppError);
            ErrorMessage = ExceptionCode.AppError;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AppException" /> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorMessage">The error message.</param>
        public AppException(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
        /// <summary>
        /// Gets or Sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or Sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public string ErrorCode { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="AppException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public AppException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }

        public override string ToString()
        {
            return $"ErrorCode:{this.ErrorCode},ErrorMessage:{ErrorMessage},ExceptionMessage:{this.Message}";
        }
    }
}