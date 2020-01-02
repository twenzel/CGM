using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=3
    /// </summary>
    public class PolyMarker : Command
    {
        public CgmPoint[] Points { get; set; }

        public PolyMarker(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 3, container))
        {

        }

        public PolyMarker(CgmFile container, CgmPoint[] points)
            : this(container)
        {
            Points = points;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var n = reader.Arguments.Length / reader.SizeOfPoint();

            Points = new CgmPoint[n];

            for (var i = 0; i < n; i++)
                Points[i] = reader.ReadPoint();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach (var p in Points)
                writer.WritePoint(p);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" MARKER");

            foreach (var p in Points)
                writer.Write($" {WritePoint(p)}");

            writer.WriteLine(";");
        }

        public override string ToString()
        {
            return "PolyMarker " + string.Join<CgmPoint>(", ", Points);
        }
    }
}
