using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=16
    /// </summary>
    public class CircularArcCentreClose : CircularArcCentre
    {
        public ClosureType Type { get; private set; }

        public CircularArcCentreClose(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 16, container))
        {

        }

        public CircularArcCentreClose(CGMFile container, CGMPoint center, double startDeltaX, double startDeltaY, double endDeltaX, double endDeltaY, double radius, ClosureType closure)
            : this(container)
        {
            SetValues(center, startDeltaX, startDeltaY, endDeltaX, endDeltaY, radius);
            Type = closure;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            base.ReadFromBinary(reader);

            var type = reader.ReadEnum();
            if (type == 0)
            {
                Type = ClosureType.PIE;
            }
            else if (type == 1)
            {
                Type = ClosureType.CHORD;
            }
            else
            {
                reader.Unsupported("unsupported closure type " + type);
                Type = ClosureType.CHORD;
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            base.WriteAsBinary(writer);
            writer.WriteEnum((int)Type);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write("  ARCCTRCLOSE ");
            WriteValues(writer);
            writer.Write($" {WriteEnum(Type)}");
            writer.WriteLine(";");
        }
    }
}