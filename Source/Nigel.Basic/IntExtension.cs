using System;
using System.Collections.Generic;
using System.Text;

namespace Nigel.Basic
{
    public static class IntExtension
    {
        /// <summary>
        /// Converts the string representation of a number to an integer.
        /// </summary>
        /// <param name="intVal">The int value.</param>
        /// <returns>System.Int32.</returns>
        public static bool ToBool(this int intVal)
        {
            return intVal > 0;
        }
    }
}
