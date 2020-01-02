using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=23
    /// </summary>
    public class ParabolicArc : Command
    {
        public CgmPoint IntersectionPoint { get; set; }
        public CgmPoint Start { get; set; }
        public CgmPoint End { get; set; }

        public ParabolicArc(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 23, container))
        {

        }

        public ParabolicArc(CgmFile container, CgmPoint intersectionPoint, CgmPoint start, CgmPoint end)
            : this(container)
        {
            IntersectionPoint = intersectionPoint;
            Start = start;
            End = end;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            IntersectionPoint = reader.ReadPoint();
            Start = reader.ReadPoint();
            End = reader.ReadPoint();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(IntersectionPoint);
            writer.WritePoint(Start);
            writer.WritePoint(End);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" PARABARC {WritePoint(IntersectionPoint)} {WritePoint(Start)} {WritePoint(End)};");
        }
    }
}
