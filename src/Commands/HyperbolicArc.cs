using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=22
    /// </summary>
    public class HyperbolicArc : Command
    {
        public CGMPoint Center { get; set; }
        public CGMPoint TransverseRadius { get; set; }
        public CGMPoint ConjugateRadius { get; set; }
        public double StartX { get; set; }
        public double StartY { get; set; }
        public double EndX { get; set; }
        public double EndY { get; set; }

        public HyperbolicArc(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 22, container))
        {

        }

        public HyperbolicArc(CGMFile container, CGMPoint center, CGMPoint transverseRadius, CGMPoint conjugateRadius, double startX, double startY, double endX, double endY)
            : this(container)
        {
            Center = center;
            TransverseRadius = transverseRadius;
            ConjugateRadius = conjugateRadius;
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Center = reader.ReadPoint();
            TransverseRadius = reader.ReadPoint();
            ConjugateRadius = reader.ReadPoint();
            StartX = reader.ReadVdc();
            StartY = reader.ReadVdc();
            EndX = reader.ReadVdc();
            EndY = reader.ReadVdc();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(Center);
            writer.WritePoint(TransverseRadius);
            writer.WritePoint(ConjugateRadius);
            writer.WriteVdc(StartX);
            writer.WriteVdc(StartY);
            writer.WriteVdc(EndX);
            writer.WriteVdc(EndY);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" HYPERBARC {WritePoint(Center)} {WritePoint(TransverseRadius)} {WritePoint(ConjugateRadius)} {WriteVDC(StartX)} {WriteVDC(StartY)} {WriteVDC(EndX)} {WriteVDC(EndY)};");
        }
    }
}
