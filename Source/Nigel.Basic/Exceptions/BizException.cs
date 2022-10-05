using System;
using System.Globalization;

namespace Nigel.Basic.Exceptions
{
    /// <summary>
    /// Class AppException.
    /// </summary>
    /// <seealso cref="Exception" />
    public class BizException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BizException"/> class.
        /// </summary>
        public BizException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BizException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BizException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BizException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public BizException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}