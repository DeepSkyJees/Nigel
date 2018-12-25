namespace Nigel.Basic
{
    public static class BoolExtension
    {
        /// <summary>
        ///     Converts the string representation of a number to an integer.
        /// </summary>
        /// <param name="boolVal">if set to <c>true</c> [bool value].</param>
        /// <returns>System.Int32.</returns>
        public static int ToInt(this bool boolVal)
        {
            if (boolVal) return 1;

            return 0;
        }
    }
}