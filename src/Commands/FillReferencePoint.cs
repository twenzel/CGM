using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=31
    /// </summary>
    public class FillReferencePoint : Command
    {
        public CGMPoint Point { get; set; }

        public FillReferencePoint(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 31, container))
        {

        }

        public FillReferencePoint(CGMFile container, CGMPoint point)
            : this(container)
        {
            Point = point;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Point = reader.ReadPoint();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(Point);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" FILLREFPT {WritePoint(Point)};");
        }
    }
}