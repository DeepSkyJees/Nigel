using System;
using System.Collections.Generic;
using System.Text;
using Nigel.Basic.Exceptions;

namespace Nigel.Basic
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string s)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrEmpty(s))
            {
                return false;
            }

            return true;
        }

        public static bool IsNotNullOrEmpty(this string s)
        {
            return !s.IsNullOrEmpty();
        }


        public static DateTime ToDateTime(this string dateTimeString)
        {
            var isDateTime = DateTime.TryParse(dateTimeString, out DateTime dt);
            if (isDateTime)
            {
                return dt;
            }

            if (dateTimeString.Length == 8 || dateTimeString.Length == 14)
            {
                return NewDateTime(dateTimeString, dateTimeString.Length);
            }
            throw new TypeConvertException("Invalid date string");
        }

        /// <summary>
        /// News the date time.
        /// </summary>
        /// <param name="dateTimeString">The date time string.</param>
        /// <param name="length">The length.</param>
        /// <returns>DateTime.</returns>
        /// <exception cref="Nigel.Basic.Exceptions.TypeConvertException">
        /// 无效的年份
        /// or
        /// 无效的月份
        /// or
        /// 无效的天数
        /// or
        /// 无效的小时
        /// or
        /// 无效的分钟
        /// or
        /// 无效的秒数
        /// </exception>
        private static DateTime NewDateTime(string dateTimeString, int length)
        {
            int year = dateTimeString.Substring(0, 4).ToInt();
            if (year < 1900 || year > System.DateTime.MaxValue.Year)
            {
                throw new TypeConvertException("无效的年份");
            }

            var month = dateTimeString.Substring(4, 2).ToInt();
            if (month < 1 || month > 12)
            {
                throw new TypeConvertException("无效的月份");
            }

            int days = System.DateTime.DaysInMonth(year, month);
            var day = dateTimeString.Substring(6, 2).ToInt();
            if (day < 1 || month > days)
            {
                throw new TypeConvertException("无效的天数");
            }

            if (length == 14)
            {
                int hour = dateTimeString.Substring(8, 2).ToInt();
                if (hour < 0 || month > 23)
                {
                    throw new TypeConvertException("无效的小时");
                }
                int minute = dateTimeString.Substring(10, 2).ToInt();
                if (minute < 0 || minute > 59)
                {
                    throw new TypeConvertException("无效的分钟");
                }
                int second = dateTimeString.Substring(12, 2).ToInt();
                if (second < 0 || second > 59)
                {
                    throw new TypeConvertException("无效的秒数");
                }
                return new DateTime(year, month, day, hour, minute, second);
            }
            return new DateTime(year, month, day);
        }


        public static int ToInt(this string intString)
        {
            var isInt = Int32.TryParse(intString, out int intResult);
            if (isInt)
            {
                return intResult;
            }
            throw new TypeConvertException("Type can not convert");
        }
        public static decimal ToDecimal(this string decimalString)
        {
            var isDecimal = decimal.TryParse(decimalString, out decimal decimalResult);
            if (isDecimal)
            {
                return decimalResult;
            }
            throw new TypeConvertException("Type can not convert");
        }


        public static double ToDouble(this string doubleString)
        {
            var isDouble = double.TryParse(doubleString, out double doubleResult);
            if (isDouble)
            {
                return doubleResult;
            }
            throw new TypeConvertException("Type can not convert");
        }

        public static float ToFloat(this string doubleString)
        {
            var isFloat = float.TryParse(doubleString, out float floatResult);
            if (isFloat)
            {
                return floatResult;
            }
            throw new TypeConvertException("Type can not convert");
        }


        /// <summary>
        /// Formats the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="paramObjects">The parameter objects.</param>
        /// <returns>System.String.</returns>
        public static string Format(this string str, params object[] paramObjects)
        {
            return string.Format(str, paramObjects);
        }

    }
}
