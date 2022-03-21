using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nigel.Basic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Nigel.Basic
{
    public static class StringExtension
    {
        [Obsolete]
        public static bool IsNullOrEmpty(this string s)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrEmpty(s)) return true;

            return false;
        }

        public static bool IsEmptyString(this string s)
        {
            if (string.IsNullOrEmpty(s)) return true;

            return false;
        }

        public static bool IsNoneValue(this string s)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrEmpty(s)) return true;

            return false;
        }

        [Obsolete]
        public static bool IsNotNullOrEmpty(this string s)
        {
            return !s.IsNullOrEmpty();
        }

        public static bool HasTrueValue(this string s)
        {
            return !s.IsNoneValue();
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

        public static bool Contains(this String str, String substring,
                                   StringComparison comp)
        {
            if (substring == null)
                throw new ArgumentNullException("substring",
                                                "substring cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), comp))
                throw new ArgumentException("comp is not a member of StringComparison",
                                            "comp");

            return str.IndexOf(substring, comp) >= 0;
        }

        public static bool Contains(this String str, String substring)
        {
            if (substring == null)
                throw new ArgumentNullException("substring",
                                                "substring cannot be null.");
            return str.IndexOf(substring) >= 0;
        }

        public static Guid ToGuid(this string guidString)
        {
            var convartResult = Guid.TryParse(guidString, out Guid guid);
            if (convartResult)
                return guid;
            return Guid.Empty;
        }

        /// <summary>
        /// 不为Null,空字符串、空格
        /// </summary>
        /// <param name="strValue">The string value.</param>
        /// <returns>
        ///   <c>true</c> if [is not null all] [the specified string value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNullAll(this string strValue)
        {
            return !string.IsNullOrEmpty(strValue) && !string.IsNullOrWhiteSpace(strValue);
        }

        public static string ChangeStringDouble(this string param, int decimals)
        {
            try
            {
                if (param == null)
                    return "";
                var numString = string.Empty;
                var charString = string.Empty;
                foreach (var c in param)
                    if (c >= 48 && c <= 57 || c == 46)
                        numString += c;
                    else
                        charString += c;

                var number = Math.Round(double.Parse(numString), decimals);
                return number + charString;
            }
            catch (Exception e)
            {
                return param;
            }
        }

        public static double ToDouble(this string param, int decimals)
        {
            if (param == null)
                return 0;
            var numString = string.Empty;
            var charString = string.Empty;
            foreach (var c in param)
                if (c >= 48 && c <= 57 || c == 46)
                    numString += c;
                else
                    charString += c;

            var number = Math.Round(double.Parse(numString), decimals);
            return number;
        }

        public static double GetNumber(this string param)
        {
            /**  \\d+\\.?\\d*
            * \d 表示数字
            * + 表示前面的数字有一个或多个（至少出现一次）
            * \. 此处需要注意，. 表示任何原子，此处进行转义，表示单纯的 小数点
            * ? 表示0个或1个
            * * 表示0次或者多次
            */
            var r = new Regex("\\d+\\.?\\d*");
            var ismatch = r.IsMatch(param);
            if (ismatch)
            {
                var mc = r.Matches(param);
                var result = mc[0].Value;
                return Convert.ToDouble(result);
            }

            throw new Exception("未获取到数字类型");
        }

        public static string RemoveNumber(this string key)
        {
            return Regex.Replace(key, @"\d", "");
        }

        /// <summary>
        /// 单词变成复数形式
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToPlural(this string word)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
            Regex plural3 = new Regex("(?<keep>[sxzh])$");
            Regex plural4 = new Regex("(?<keep>[^sxzhy])$");

            if (plural1.IsMatch(word))
                return plural1.Replace(word, "${keep}ies");
            if (plural2.IsMatch(word))
                return plural2.Replace(word, "${keep}s");
            if (plural3.IsMatch(word))
                return plural3.Replace(word, "${keep}es");
            if (plural4.IsMatch(word))
                return plural4.Replace(word, "${keep}s");

            return word;
        }


        /// <summary>
        /// 计算字符串表达式的值。
        /// </summary>
        /// <param name="exp">输入的表达式字符串</param>
        /// <returns>返回计算值，为double类型</returns>
        public static double Compute(this string exp)
        {
            if (!checkRules(exp))
            {
                throw new FormatException("字符串为空或不合法");
            }

            //先把字符串转换成后缀表达式字符串数组
            string[] rpn = toRPN(toStrings(exp));

            //再计算后缀表达式
            Stack<double> stack = new Stack<double>(); //存放参与计算的数值、中间值、结果

            //算法：利用foreach来扫描后缀表达式字符串数组，得到数值则直接压入栈中，
            //得到运算符则从栈顶取出两个数值进行运算，并把结果压入栈中。最终栈中留下一个数值，为计算结果。
            foreach (string oprator in rpn)
            {
                //为什么总是弹出两个数值？因为都是双目运算。
                //先弹出的是运算符右边的数，弹出两个数值后注意运算顺序。
                switch (oprator)
                {
                    case "+":
                        //如果读取到运算符，则从stack中取出两个数值进行运算，并把运算结果压入stack。下同。
                        stack.Push(stack.Pop() + stack.Pop());
                        break;

                    case "-":
                        stack.Push(-stack.Pop() + stack.Pop());
                        break;

                    case "*":
                        stack.Push(stack.Pop() * stack.Pop());
                        break;

                    case "/":
                        {
                            double right = stack.Pop();
                            try
                            {
                                stack.Push(stack.Pop() / right);
                            }
                            catch (Exception e)
                            {
                                throw e;   //除数为0时产生异常。
                            }

                            break;
                        }
                    case "^":
                        {
                            double right = stack.Pop();
                            stack.Push(Math.Pow(stack.Pop(), right));
                            break;
                        }

                    default: //后缀表达式数组中只有运算符和数值，没有圆括号。除了运算符，剩下的就是数值了
                        stack.Push(double.Parse(oprator));  //如果读取到数值，则压入stack中
                        break;
                }
            }

            //弹出最后的计算值并返回
            return stack.Pop();
        }

        /// <summary>
        /// 检查字符串，判断是否满足表达式的语法要求。
        /// </summary>
        /// <param name="exp">表达式字符串</param>
        /// <returns>字符串为空或不满足表达式语法要求时返回false，否则返回true</returns>
        public static bool checkRules(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp))
            {
                return false;
            }

            //去掉空格
            string noBlank = Regex.Replace(exp, " ", "");

            //Console.WriteLine(noBlank);

            //表达式字符串规则。规则之间有配合关系，以避免重叠。
            string no0 = @"[^\d\*\^\(\)+-/.]";  //规则0：不能出现运算符+-*/^、圆括号()、数字、小数点.之外的字符
            string no1 = @"^[^\d\(-]";          //规则1：开头不能是数字、左圆括号(、负号- 以外的字符
            string no2 = @"[^\d\)]$";           //规则2：结束不能是数字、右圆括号) 以外的字符
            string no3 = @"[\*\^+-/]{2}";       //规则3：+-*/^不能连续出现
            string no4 = @"[\D][.]|[.]\D";      //规则4：小数点前面或后面不能出现非数字字符
            string no5 = @"\)[\d\(]|[^\d\)]\)"; //规则5：右圆括号)后面不能出现数字、左圆括号(,前面不能出现除数字或右圆括号)以外的字符
            string no6 = @"\([^\d\(-]|[\d]\(";  //规则6：左圆括号(后面不能出现除数字、左圆括号(、负号以外的字符,前面不能出现数字

            string pattern = no0 + "|" + no1 + "|" + no2 + "|" + no3 + "|" + no4 + "|" + no5 + "|" + no6;
            if (Regex.IsMatch(noBlank, pattern))
            {
                return false;
            }

            //规则7：左圆括号(和右圆括号)必须成对出现
            int count = 0;
            foreach (char c in noBlank)
            {
                if (c == ')')
                {
                    count++;
                    continue;
                }

                if (c == '(')
                {
                    count--;
                    continue;
                }
            }
            if (count != 0)
            {
                Console.WriteLine("左右括号不匹配");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 将表达式字符串转换成字符串数组，如"-2*57+6"转换成["-2","*","57","+","6"]。请自行确保表达式字符串正确。
        /// </summary>
        /// <param name="exp">表达式字符串</param>
        /// <returns>字符串数组</returns>
        public static string[] toStrings(string exp)
        {
            //通过添加分割符','把数字和其它字符分隔开。

            StringBuilder sb = new StringBuilder(exp);

            //去掉空格
            sb.Replace(" ", "");

            //负号与减号相同，不能与数字分开，需要特许处理：
            //1、字符串第一个"-"是负号
            //2、紧跟在"("后面的"-"是负号
            //3、如果负号后面直接跟着"("，把负号改为"-1*"。目的是把取负运算（单目运算）变成乘法运算（双目运算），免得以后要区分减法和取负。
            //4、把负号统一改为"?"
            if (sb[0] == '-')
            {
                sb[0] = '?';
            }

            sb.Replace("(-", "(?");

            sb.Replace("?(", "?1*(");

            //添加分割符','把数字和其它字符分隔开。
            sb.Replace("+", ",+,");
            sb.Replace("-", ",-,");
            sb.Replace("*", ",*,");
            sb.Replace("/", ",/,");
            sb.Replace("(", "(,");
            sb.Replace(")", ",)");
            sb.Replace("^", ",^,");

            //分割之后，把'?'恢复成减号 '-'
            sb.Replace('?', '-');

            return sb.ToString().Split(',');
        }

        /// <summary>
        /// 生成后波兰表达式（后缀表达式）
        /// </summary>
        /// <param name="expStrings">字符串数组（中缀表达式）</param>
        /// <returns>字符串数组（后缀表达式）</returns>
        private static string[] toRPN(string[] expStrings)
        {
            Stack<string> stack = new Stack<string>();
            List<string> rpn = new List<string>();

            //基本思路：
            //遍历expStrings中的字符串：
            //1、如果不是字符（即数字）就直接放到列表rpn中；
            //2、如果是字符：
            //2.1、如果stack为空，把字符压入stack中；
            //2.2、如果stack不为空，把栈中优先级大于等于该字符的运算符全部弹出(直到碰到'('或stack为空)，放到rpn中；
            //2.2 如果字符是'('，直接压入
            //2.3 如果是')'，依次弹出stack中的字符串放入rpn中，直到碰到'(',弹出并抛弃'('；
            foreach (string item in expStrings)
            {
                //1、处理"("
                if (item == "(")
                {
                    stack.Push(item);
                    continue;
                }

                //2、处理运算符 +-*/^
                if ("+-*/^".Contains(item))
                {
                    if (stack.Count == 0)
                    {
                        stack.Push(item);
                        continue;
                    }

                    if (getOrder(item[0]) > getOrder(stack.Peek()[0]))
                    {
                        stack.Push(item);
                        continue;
                    }
                    else
                    {
                        while (stack.Count > 0 && getOrder(stack.Peek()[0]) >= getOrder(item[0]) && stack.Peek() != "(")
                        {
                            rpn.Add(stack.Pop());
                        }
                        stack.Push(item);
                        continue;
                    }
                }

                //3、处理")"
                if (item == ")")
                {
                    while (stack.Peek() != "(")
                    {
                        rpn.Add(stack.Pop());
                    }
                    stack.Pop();//抛弃"("
                    continue;
                }

                //4、数据，直接放入rpn中
                rpn.Add(item);
            }

            //最后把stack中的运算符全部输出到rpn
            while (stack.Count > 0)
            {
                rpn.Add(stack.Pop());
            }

            //把字符串链表转换成字符串数组，并输出。
            return rpn.ToArray();
        }

        /// <summary>
        /// 获取运算符的优先级
        /// </summary>
        /// <param name="oprator">运算符</param>
        /// <returns>运算符的优先级。</returns>
        private static int getOrder(char oprator)
        {
            switch (oprator)
            {
                case '+':
                case '-':
                    return 1;

                case '*':
                case '/':
                    return 3;

                case '^':
                    return 5;
                //case '(':
                //	return 10;
                default:
                    return -1;
            }
        }
    }
}