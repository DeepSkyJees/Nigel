using System.Net;

namespace Nigel.Basic
{
    public class ResponseData
    {
        public static ResponseData<T> SetResult<T>(T tData, HttpStatusCode httpStatusCode = HttpStatusCode.OK, string message = "Successful")
        {
            return new ResponseData<T>
            {
                Data = tData,
                Code = httpStatusCode,
                Message = message,
                State = true
            };
        }
    }

    public class ResponseData<T>
    {
        public T Data { get; set; }

        public HttpStatusCode Code { get; set; }

        public bool State { get; set; }

        public string Message { get; set; }
    }
}