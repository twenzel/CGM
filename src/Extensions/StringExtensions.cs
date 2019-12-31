namespace codessentials.CGM
{
    /// <summary>
    /// Extension methods for strings
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Replaces string ignoring case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="valueToSearch">The value to search.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static string ReplaceIgnoreCase(this string value, string valueToSearch, string replacement)
        {
            if (!string.IsNullOrEmpty(valueToSearch))
            {
                var lastIndex = -1;
                int index;
                while ((index = value.IndexOf(valueToSearch, lastIndex + 1, System.StringComparison.OrdinalIgnoreCase)) >= 0)
                {
                    value = value.Remove(index, valueToSearch.Length);
                    value = value.Insert(index, replacement);
                    lastIndex = index;
                }
            }

            return value;
        }

        /// <summary>
        /// Equals the strings ignoring the casing
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="checkValue">The check value.</param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string value, string checkValue)
        {
            if (value == null)
                return checkValue == null;
            else
                return value.Equals(checkValue, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
