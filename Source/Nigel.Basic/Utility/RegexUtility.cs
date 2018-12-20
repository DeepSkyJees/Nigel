using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Nigel.Basic.Utility
{
    /// <summary>
    ///     RegexUtility.
    /// </summary>
    public static class RegexUtility
    {
        /// <summary>
        ///     A regular expression for validating cellphone number.
        /// </summary>
        public static readonly Regex CellphoneRegex = new Regex(@"^(13|14|15|17|18)\d{9}$");

        /// <summary>
        ///     The cellphone regex string
        /// </summary>
        public static readonly string CellphoneRegexString = CellphoneRegex.ToString();

        /// <summary>
        ///     A regular expression for validating Email Addresses. Taken from http://net.tutsplus.com/tutorials/other/8-regular-expressions-you-should-know/
        /// </summary>
        public static readonly Regex EmailRegex = new Regex(@"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$");

        /// <summary>
        ///     The email regex string
        /// </summary>
        public static readonly string EmailRegexString = EmailRegex.ToString();

        /// <summary>
        ///     A regular expression for validating IPAddresses. Taken from http://net.tutsplus.com/tutorials/other/8-regular-expressions-you-should-know/
        /// </summary>
        public static readonly Regex IPAddressRegex = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

        /// <summary>
        ///     The ip address regex string
        /// </summary>
        public static readonly string IPAddressRegexString = IPAddressRegex.ToString();

        /// <summary>
        ///     A regular expression for validating that string is a positive number GREATER THAN zero.
        /// </summary>
        public static readonly Regex PositiveNumberRegex = new Regex(@"^[1-9]+[0-9]*$");

        /// <summary>
        ///     The positive number regex string
        /// </summary>
        public static readonly string PositiveNumberRegexString = PositiveNumberRegex.ToString();

        /// <summary>
        ///     A regular expression for validating absolute Urls. Taken from https://mathiasbynens.be/demo/url-regex
        /// </summary>
        public static readonly Regex UrlRegex = new Regex(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$");

        /// <summary>
        ///     The URL regex string
        /// </summary>
        public static readonly string UrlRegexString = UrlRegex.ToString();
    }
}
