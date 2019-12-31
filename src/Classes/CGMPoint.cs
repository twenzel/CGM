using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Classes
{
    /// <summary>
    /// Represents a point parameter type
    /// </summary>
    public class CGMPoint : IEquatable<CGMPoint>, IComparable<CGMPoint>
    {
        public double X { get; private set; }
        public double Y { get; private set; }        

        public CGMPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"Point({X}, {Y})";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CGMPoint);           
        }

        public bool Equals(CGMPoint other)
        {
            if (other == null)
                return false;

            return IsSame(other.X,X) && IsSame(other.Y, Y);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// sort CGMPoints to the leftest upper corner
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int CompareTo(CGMPoint other)
        {
            var calc = CompareValues(X, other.X);

            if (calc == 0)
                return CompareValues(Y, other.Y);
            else
                return calc;
        }

        public static bool IsSame(double x, double y)
        {
            return CompareValues(x, y) == 0;
        }

        public static int CompareValues(double x, double y)
        {
            x = Math.Round(x, 4);
            y = Math.Round(y, 4);

            if (x == y)
                return 0;
            else if (Math.Abs(x - y) < 0.0004)
                return 0;
            else
                return x.CompareTo(y);                
        }
    }

    /// <summary>
    /// Comparer to sort CGMPoints to the leftest upper corner
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IComparer{codessentials.CGM.Classes.CGMPoint}" />
    public class CGMPointComparer : IComparer<CGMPoint>
    {
        public int Compare(CGMPoint x, CGMPoint y)
        {
            return x.CompareTo(y);
        }
    }
}
