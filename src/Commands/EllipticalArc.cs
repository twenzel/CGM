using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=18
    /// </summary>
    public class EllipticalArc : EllipseElement
    {
        public double StartVectorDeltaX { get; set; }
        public double StartVectorDeltaY { get; set; }
        public double EndVectorDeltaX { get; set; }
        public double EndVectorDeltaY { get; set; }

        public EllipticalArc(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 18, container))
        {

        }

        public EllipticalArc(CommandConstructorArguments args)
           : base(args)
        {

        }

        public EllipticalArc(CGMFile container, double startX, double startY, double endX, double endY, CGMPoint center, CGMPoint first, CGMPoint second)
            : this(container)
        {
            SetValues(startX, startY, endX, endY, center, first, second);
        }

        protected void SetValues(double startX, double startY, double endX, double endY, CGMPoint center, CGMPoint first, CGMPoint second)
        {
            SetValues(center, first, second);
            StartVectorDeltaX = startX;
            StartVectorDeltaY = startY;
            EndVectorDeltaX = endX;
            EndVectorDeltaY = endY;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            base.ReadFromBinary(reader);

            StartVectorDeltaX = reader.ReadVdc();
            StartVectorDeltaY = reader.ReadVdc();
            EndVectorDeltaX = reader.ReadVdc();
            EndVectorDeltaY = reader.ReadVdc();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            base.WriteAsBinary(writer);
            writer.WriteVdc(StartVectorDeltaX);
            writer.WriteVdc(StartVectorDeltaY);
            writer.WriteVdc(EndVectorDeltaX);
            writer.WriteVdc(EndVectorDeltaY);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write("  ELLIPARC");
            WriteValues(writer);
            writer.WriteLine(";");
        }

        protected override void WriteValues(IClearTextWriter writer)
        {
            base.WriteValues(writer);
            writer.Write($" {WritePoint(StartVectorDeltaX, StartVectorDeltaY)}");
            writer.Write($" {WritePoint(EndVectorDeltaX, EndVectorDeltaY)}");
        }
    }
}