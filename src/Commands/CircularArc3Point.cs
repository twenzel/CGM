using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=13
    /// </summary>
    public class CircularArc3Point : Command
    {
        public CGMPoint P1 { get; set; }
        public CGMPoint P2 { get; set; }
        public CGMPoint P3 { get; set; }

        public CircularArc3Point(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 13, container))
        {

        }

        public CircularArc3Point(CGMFile container, CGMPoint p1, CGMPoint p2, CGMPoint p3)
            : this(container)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        public CircularArc3Point(CommandConstructorArguments args)
           : base(args)
        {

        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            P1 = reader.ReadPoint();
            P2 = reader.ReadPoint();
            P3 = reader.ReadPoint();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(P1);
            writer.WritePoint(P2);
            writer.WritePoint(P3);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" ARC3PT {WriteThreePointArcSpec()};");
        }

        protected string WriteThreePointArcSpec()
        {
            return $"{WritePoint(P1)} {WritePoint(P2)} {WritePoint(P2)}";
        }
    }
}