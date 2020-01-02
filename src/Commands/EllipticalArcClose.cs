using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=19
    /// </summary>
    public class EllipticalArcClose : EllipticalArc
    {
        public ClosureType ClosureType { get; set; }

        public EllipticalArcClose(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 19, container))
        {

        }

        public EllipticalArcClose(CgmFile container, ClosureType type, double startX, double startY, double endX, double endY, CgmPoint center, CgmPoint first, CgmPoint second)
            : this(container)
        {
            ClosureType = type;
            SetValues(startX, startY, endX, endY, center, first, second);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            base.ReadFromBinary(reader);

            var type = reader.ReadEnum();
            if (type == 0)
            {
                ClosureType = ClosureType.PIE;
            }
            else if (type == 1)
            {
                ClosureType = ClosureType.CHORD;
            }
            else
            {
                reader.Unsupported("unsupported closure type " + type);
                ClosureType = ClosureType.CHORD;
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            base.WriteAsBinary(writer);
            writer.WriteEnum((int)ClosureType);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write("  ELLIPARCCLOSE");
            WriteValues(writer);
            writer.WriteLine(";");
        }

        protected override void WriteValues(IClearTextWriter writer)
        {
            base.WriteValues(writer);
            writer.Write($" {ClosureType.ToString().ToLower()})");
        }
    }
}