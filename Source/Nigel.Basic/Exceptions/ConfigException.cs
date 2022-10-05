using System;
using System.Globalization;

namespace Nigel.Basic.Exceptions
{
    public class ConfigException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BizException"/> class.
        /// </summary>
        public ConfigException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BizException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConfigException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BizException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public ConfigException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}