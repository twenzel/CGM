namespace codessentials.CGM.Classes
{
    /// <summary>
    /// Describes a line
    /// </summary>
    internal struct CgmLine
    {
        public CgmPoint A { get; private set; }
        public CgmPoint B { get; private set; }

        public CgmLine(CgmPoint a, CgmPoint b)
        {
            if (a.X < b.X)
            {
                A = a;
                B = b;
            }
            else if (CgmPoint.IsSame(a.X, b.X))
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
