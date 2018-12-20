using System;

namespace Nigel.Basic
{
    public static class DateTimeExtend
    {
        /// <summary>
        ///     Tps the china date time.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ToChinaDateTime(this DateTime dt)
        {
            return dt.ToUniversalTime().AddHours(8);
        }
    }
}