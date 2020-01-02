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
                A = a;
                B = b;
            }
            else if (CGMPoint.IsSame(a.X, b.X))
            {
                if (a.Y < b.Y)
                {
                    A = a;
                    B = b;
                }
                else
                {
                    A = b;
                    B = a;
                }
            }
            else
            {
                A = b;
                B = a;
            }
        }
    }
}
