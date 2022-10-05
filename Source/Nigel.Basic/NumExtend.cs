using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Basic
{
    public static class NumExtend
    {
        public static bool ToBool(this int intVal)
        {
            return intVal > 0;
        }

        /// <summary>
        /// 保留小数位
        /// </summary>
        /// <param name="val"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static double ToKeepDigit(this double val, int digit)
        {
            return Math.Round(val, digit);
        }
    }
}
