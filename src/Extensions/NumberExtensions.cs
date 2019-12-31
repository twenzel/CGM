using System;

namespace codessentials.CGM
{
    internal static class NumberExtensions
    {
        /// <summary>
        /// Gets the decimals before the separator
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetWholePart(this double value)
        {
            return (int)Math.Truncate(value);
        }

        /// <summary>
        /// Gets the decimals after the separator (e.g. 10.2 -> 0.2)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double GetFractionalPart(this double value)
        {
            return value - Math.Truncate(value);
        }
    }
}
