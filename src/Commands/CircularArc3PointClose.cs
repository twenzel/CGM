using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=14
    /// </summary>
    public class CircularArc3PointClose : CircularArc3Point
    {
        public ClosureType Type { get; private set; }

        public CircularArc3PointClose(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 14, container))
        {

        }

        public CircularArc3PointClose(CgmFile container, CgmPoint p1, CgmPoint p2, CgmPoint p3, ClosureType closureType)
            : this(container)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Type = closureType;
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
            writer.WriteLine($" ARC3PTCLOSE {WriteThreePointArcSpec()} {WriteEnum(Type)};");
        }
    }
}