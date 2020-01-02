using System;
using System.Drawing;

namespace codessentials.CGM.Classes
{
    /// <summary>
    /// Represents a color parameter type
    /// </summary>
    public class CGMColor : IEquatable<CGMColor>
    {
        public Color Color { get; set; } = Color.Empty;
        public int ColorIndex { get; set; } = -1;

        public bool Equals(CGMColor other)
        {
            return ColorIndex == other.ColorIndex && Color.ToArgb() == other.Color.ToArgb();
        }

        public override bool Equals(object obj)
        {
            if (obj is CGMColor color)
                return Equals(color);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return ColorIndex.GetHashCode() ^ Color.GetHashCode();
        }

        public override string ToString()
        {
            return ColorIndex > -1 ? $"ColorIndex {ColorIndex}" : $"Color {Color}";
        }
    }
}
