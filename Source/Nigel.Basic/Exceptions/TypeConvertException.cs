using System;

namespace Nigel.Basic.Exceptions
{
    /// <summary>
    ///     Class TypeConvertException.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class TypeConvertException : Exception
    {
        public TypeConvertException(string message)
        {
            Message = message;
            Code = (int) ExceptionCode.ConvertTypeError;
        }

        public TypeConvertException(int code, string message)
        {
            Message = message;
            Code = code;
        }


        /// <summary>
        ///     Gets a message that describes the current exception.
        /// </summary>
        /// <value>The message.</value>
        public override string Message { get; }

        /// <summary>
        ///     Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public int Code { get; set; }
    }
}