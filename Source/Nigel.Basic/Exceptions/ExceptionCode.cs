namespace Nigel.Basic.Exceptions
{
    public enum ExceptionCode
    {
        /// <summary>
        /// 认证失败
        /// </summary>
        UnauthorizedFailed = -500,
        /// <summary>
        /// 类型转换异常
        /// </summary>
        ConvertTypeError = -1000,
        /// <summary>
        /// 系统位置级别异常
        /// </summary>
        SystemUnKnownError = -9000

    }
}