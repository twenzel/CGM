using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=15
    /// </summary>
    public class CircularArcCentre : Command
    {
        public CGMPoint Center { get; protected set; }
        public double StartDeltaX { get; protected set; }
        public double StartDeltaY { get; protected set; }
        public double EndDeltaX { get; protected set; }
        public double EndDeltaY { get; protected set; }
        public double Radius { get; protected set; }

        public CircularArcCentre(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 15, container))
        {

        }

        public CircularArcCentre(CGMFile container, CGMPoint center, double startDeltaX, double startDeltaY, double endDeltaX, double endDeltaY, double radius)
           : this(container)
        {
            SetValues(center, startDeltaX, startDeltaY, endDeltaX, endDeltaY, radius);
        }

        public CircularArcCentre(CommandConstructorArguments args)
            : base(args)
        {

        }

        protected void SetValues(CGMPoint center, double startDeltaX, double startDeltaY, double endDeltaX, double endDeltaY, double radius)
        {
            Center = center;
            StartDeltaX = startDeltaX;
            StartDeltaY = startDeltaY;
            EndDeltaX = endDeltaX;
            EndDeltaY = endDeltaY;
            Radius = radius;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Center = reader.ReadPoint();
            StartDeltaX = reader.ReadVdc();
            StartDeltaY = reader.ReadVdc();
            EndDeltaX = reader.ReadVdc();
            EndDeltaY = reader.ReadVdc();
            Radius = reader.ReadVdc();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(Center);
            writer.WriteVdc(StartDeltaX);
            writer.WriteVdc(StartDeltaY);
            writer.WriteVdc(EndDeltaX);
            writer.WriteVdc(EndDeltaY);
            writer.WriteVdc(Radius);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write("  ARCCTR");
            WriteValues(writer);
            writer.WriteLine(" ;");
        }

        protected virtual void WriteValues(IClearTextWriter writer)
        {
            writer.Write($" {WritePoint(Center)}");
            writer.Write($" {WritePoint(StartDeltaX, StartDeltaY)} {WritePoint(EndDeltaX, EndDeltaY)}");
            writer.Write($" {WriteVDC(Radius)}");
        }
    }
}