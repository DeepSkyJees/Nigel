using System;
using System.Globalization;

namespace Nigel.Basic.Exceptions
{
    public class ConfigException : AppException
    {
        public ConfigException():base(nameof(ExceptionCode.ConfigError), ExceptionCode.ConfigError)
        { 
        }

        public ConfigException(string errorMessage)
        {
            ErrorMessage = errorMessage.IsNotNullAll()
                ? $"{ExceptionCode.ConfigError},{errorMessage}"
                : ExceptionCode.ConfigError; ;
            ErrorCode = nameof(ExceptionCode.ConfigError);
        }
    }
}