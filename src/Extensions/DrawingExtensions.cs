using System.Drawing;

namespace codessentials.CGM
{
    /// <summary>
    /// Drawing utilities
    /// </summary>
    internal static class DrawingExtensions
    {
        /// <summary>
        /// Determines whether the colors are equal
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="othercolor">The othercolor.</param>
        /// <returns></returns>
        public static bool EqualsColor(this Color color, Color othercolor)
        {
            return color.ToArgb().Equals(othercolor.ToArgb());
        }
    }
}
