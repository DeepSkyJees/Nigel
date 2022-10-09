using System;

namespace Nigel.Basic.Exceptions
{
    /// <summary>
    ///     Class TypeConvertException.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class TypeConvertException : AppException
    {
        public TypeConvertException(string errorMessage)
        {
            ErrorMessage = errorMessage.IsNotNullAll()
                ? $"{ExceptionCode.ConvertTypeError},{errorMessage}"
                : ExceptionCode.ConvertTypeError; ;
            ErrorCode = nameof(ExceptionCode.ConvertTypeError);
        }
        public TypeConvertException():base(nameof(ExceptionCode.ConvertTypeError),ExceptionCode.ConvertTypeError)
        {
        }
    }
}