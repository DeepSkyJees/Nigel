using System.Net;
using Nigel.Basic.Exceptions;

namespace Nigel.Extensions.AspNetCore
{
    public class ApiResponseResult
    {
        /// <summary>
        /// Gets the ok result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <returns>
        /// ModelResult&lt;T&gt;.
        /// </returns>
        public static ApiResponseResult<T> GetResponseResult<T>(T data, int httpStatusCode = (int)HttpStatusCode.OK)
        {
            return new ApiResponseResult<T>
            {
                Data = data,
                HttpStatusCode = httpStatusCode,
                ResponseState = true
            };
        }
        public static ApiErrResponseResult GetErrorResponseResult(int httpStatusCode = (int)HttpStatusCode.InternalServerError,string errorCode = nameof(ExceptionCode.SystemUnKnownError), string errorMessage= ExceptionCode.SystemUnKnownError)
        {
            return new ApiErrResponseResult
            {
                Data = default(object),
                HttpStatusCode = httpStatusCode,
                ResponseState = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
        }
    }

    /// <summary>
    ///     Class ModelResult.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponseResult<T>
    {
        /// <summary>
        ///     返回数据
        /// </summary>
        /// <value>The data.</value>
        public T Data { get; set; }

        /// <summary>
        ///     返回的编码
        /// </summary>
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [response state].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [response state]; otherwise, <c>false</c>.
        /// </value>
        public bool ResponseState { get; set; }
    }

    public class ApiErrResponseResult: ApiResponseResult<object>
    {
        /// <summary>
        /// Gets or Sets the message code.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        ///     结果描述
        /// </summary>
        /// <value>The message.</value>
        public string ErrorMessage { get; set; }
    }

}
