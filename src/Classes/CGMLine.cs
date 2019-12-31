using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Classes
{
    /// <summary>
    /// Describes a line
    /// </summary>
    public struct CGMLine
    {
        public CGMPoint A { get; private set; }
        public CGMPoint B { get; private set; }

        public CGMLine(CGMPoint a, CGMPoint b)
        {
            if (a.X < b.X)
            {
                this.A = a;
                this.B = b;
            }
            else if (CGMPoint.IsSame(a.X, b.X))
            {
                if (a.Y < b.Y)
                {
                    this.A = a;
                    this.B = b;
                } else
                {
                    this.A = b;
                    this.B = a;
                }
            }
            else
            {
                this.A = b;
                this.B = a;
            }
        }
    }
}
