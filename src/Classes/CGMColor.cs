using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Classes
{
    /// <summary>
    /// Represents a color parameter type
    /// </summary>
    public class CGMColor: IEquatable<CGMColor>
    {
        public Color Color { get; set; } = Color.Empty;
        public int ColorIndex { get; set; } = -1;

        public bool Equals(CGMColor other)
        {
            return ColorIndex == other.ColorIndex && Color.ToArgb() == other.Color.ToArgb();
        }

        public override bool Equals(object obj)
        {
            var color = obj as CGMColor;

            if (color != null)
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
