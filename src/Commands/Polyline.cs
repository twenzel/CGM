using System;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=1
    /// </summary>
    public class Polyline : Command, IComparable<Polyline>
    {
        public Polyline(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 1, container))
        {

        }

        public Polyline(CGMFile container, CGMPoint[] points)
            : this(container)
        {
            Points = points;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var n = reader.Arguments.Length / reader.SizeOfPoint();

            Points = new CGMPoint[n];

            for (var i = 0; i < n; i++)
            {
                Points[i] = reader.ReadPoint();
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach (var p in Points)
                writer.WritePoint(p);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write("  LINE");

            foreach (var points in Points)
                writer.Write($" {WritePoint(points)}");

            writer.WriteLine(";");
        }

        public override string ToString()
        {
            return "Polyline " + string.Join<CGMPoint>(", ", Points);
        }

        public bool IsSimpleLine
        {
            get { return Points.Length == 2; }
        }

        public CGMPoint[] Points { get; private set; }

        public int CompareTo(Polyline other)
        {
            if (Points.Length == other.Points.Length)
            {
                for (var i = 0; i < Points.Length; i++)
                {
                    var compareResult = Points[i].CompareTo(other.Points[i]);
                    if (compareResult != 0)
                        return compareResult;
                }

                return 0;
            }
            else
                return Points.Length.CompareTo(other.Points.Length);
        }
    }
}
