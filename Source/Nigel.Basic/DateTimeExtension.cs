using System;

namespace Nigel.Basic
{
    public static class DateTimeExtension
    {
        /// <summary>
        ///     将日期时间转为中国区日期时间
        ///     DateTime.Now.ToChinaDateTime()
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ToChinaDateTime(this DateTime dt)
        {
            return dt.ToUniversalTime().AddHours(8);
        }

        /// <summary>
        /// DateTime.ToChinaDateTimeFormUtc()
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static DateTime ToChinaDateTimeFormUtc(this DateTime dt)
        {
            return dt.AddHours(8);
        }

        /// <summary>
        /// Dates the time to timestamp.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns></returns>
        public static long ToTimestampFromDateTime(this DateTime datetime)
        {
            DateTime dd = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime timeUTC = DateTime.SpecifyKind(datetime, DateTimeKind.Utc);//本地时间转成UTC时间
            TimeSpan ts = (timeUTC - dd);
            return (Int64)ts.TotalMilliseconds;//精确到毫秒
        }

        /// <summary>
        /// UTCs the date time to timestamp.
        /// </summary>
        /// <param name="utcDateTime">The UTC date time.</param>
        /// <returns></returns>
        public static long ToTimestampFromUtcDateTime(this DateTime utcDateTime)
        {
            DateTime dd = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime timeUTC = utcDateTime;//本地时间转成UTC时间
            TimeSpan ts = (timeUTC - dd);
            return (Int64)ts.TotalMilliseconds;//精确到毫秒
        }
        /// <summary>
        /// 时间戳转本时区日期时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ToUtcDateTimeFromTimestamp(this string timeStamp)
        {
            DateTime dd = DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0, 0), DateTimeKind.Utc);
            long longTimeStamp = long.Parse(timeStamp + "0000");
            TimeSpan ts = new TimeSpan(longTimeStamp);
            return dd.Add(ts);
        }
    }
}