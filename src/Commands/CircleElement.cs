using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=12
    /// </summary>
    public class CircleElement : Command
    {
        public CGMPoint Center { get; private set; }
        public double Radius { get; private set; }

        public CircleElement(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 12, container))
        {

        }

        public CircleElement(CGMFile container, CGMPoint center, double radius)
            : this(container)
        {
            Center = center;
            Radius = radius;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Center = reader.ReadPoint();
            Radius = reader.ReadVdc();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(Center);
            writer.WriteVdc(Radius);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  CIRCLE {WritePoint(Center)} {WriteVDC(Radius)};");
        }

        public override string ToString()
        {
            return $"Circle [{Center.X},{Center.Y}] {Radius}";
        }
    }
}