using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nigel.Basic.Exceptions;

namespace Nigel.Basic
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string s)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrEmpty(s)) return false;

            return true;
        }

        public static bool IsNotNullOrEmpty(this string s)
        {
            return !s.IsNullOrEmpty();
        }

        /// <summary>
        ///     Determines whether [is not empty or null or white] [the specified default string].
        /// </summary>
        /// <param name="defaultString">The default string.</param>
        /// <returns><c>true</c> if [is not empty or null or white] [the specified default string]; otherwise, <c>false</c>.</returns>
        public static bool IsNotEmptyOrNullOrWhite(this string defaultString)
        {
            if (string.IsNullOrEmpty(defaultString)) return false;

            if (string.IsNullOrWhiteSpace(defaultString)) return false;

            return true;
        }



        public static DateTime ToDateTime(this string dateTimeString)
        {
            var isDateTime = DateTime.TryParse(dateTimeString, out var dt);
            if (isDateTime) return dt;

            if (dateTimeString.Length == 8 || dateTimeString.Length == 14)
                return NewDateTime(dateTimeString, dateTimeString.Length);
            throw new TypeConvertException("Invalid date string");
        }

        /// <summary>
        ///     News the date time.
        /// </summary>
        /// <param name="dateTimeString">The date time string.</param>
        /// <param name="length">The length.</param>
        /// <returns>DateTime.</returns>
        /// <exception cref="Nigel.Basic.Exceptions.TypeConvertException">
        ///     无效的年份
        ///     or
        ///     无效的月份
        ///     or
        ///     无效的天数
        ///     or
        ///     无效的小时
        ///     or
        ///     无效的分钟
        ///     or
        ///     无效的秒数
        /// </exception>
        private static DateTime NewDateTime(string dateTimeString, int length)
        {
            var year = dateTimeString.Substring(0, 4).ToInt();
            if (year < 1900 || year > DateTime.MaxValue.Year) throw new TypeConvertException("无效的年份");

            var month = dateTimeString.Substring(4, 2).ToInt();
            if (month < 1 || month > 12) throw new TypeConvertException("无效的月份");

            var days = DateTime.DaysInMonth(year, month);
            var day = dateTimeString.Substring(6, 2).ToInt();
            if (day < 1 || month > days) throw new TypeConvertException("无效的天数");

            if (length == 14)
            {
                var hour = dateTimeString.Substring(8, 2).ToInt();
                if (hour < 0 || month > 23) throw new TypeConvertException("无效的小时");
                var minute = dateTimeString.Substring(10, 2).ToInt();
                if (minute < 0 || minute > 59) throw new TypeConvertException("无效的分钟");
                var second = dateTimeString.Substring(12, 2).ToInt();
                if (second < 0 || second > 59) throw new TypeConvertException("无效的秒数");
                return new DateTime(year, month, day, hour, minute, second);
            }

            return new DateTime(year, month, day);
        }


        public static int ToInt(this string intString)
        {
            var isInt = int.TryParse(intString, out var intResult);
            if (isInt) return intResult;
            throw new TypeConvertException("Type can not convert");
        }

        public static decimal ToDecimal(this string decimalString)
        {
            var isDecimal = decimal.TryParse(decimalString, out var decimalResult);
            if (isDecimal) return decimalResult;
            throw new TypeConvertException("Type can not convert");
        }


        public static double ToDouble(this string doubleString)
        {
            var isDouble = double.TryParse(doubleString, out var doubleResult);
            if (isDouble) return doubleResult;
            throw new TypeConvertException("Type can not convert");
        }

        public static float ToFloat(this string doubleString)
        {
            var isFloat = float.TryParse(doubleString, out var floatResult);
            if (isFloat) return floatResult;
            throw new TypeConvertException("Type can not convert");
        }


        /// <summary>
        ///     Formats the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="paramObjects">The parameter objects.</param>
        /// <returns>System.String.</returns>
        public static string Format(this string str, params object[] paramObjects)
        {
            return string.Format(str, paramObjects);
        }

        public static JObject ToJObject(this string defaultString)
        {
            var jObject = JObject.Parse(defaultString);
            return jObject;
        }

        public static T To<T>(this string defaultString)
        {
            var obj = JsonConvert.DeserializeObject<T>(defaultString);
            return obj;
        }


        /// <summary>
        ///     To the list.
        /// </summary>
        /// <param name="commaSplitString">The comma split string.</param>
        /// <param name="splitChar">The split character.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> ToList(this string commaSplitString, char splitChar = ',')
        {
            var stringList = commaSplitString.Split(splitChar).ToList();
            return stringList;
        }
    }
}