using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=14
    /// </summary>
    public class CircularArc3PointClose : CircularArc3Point
    {       
        private ClosureType _closureType;

        public CircularArc3PointClose(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 14, container))
        {
            
        }

        public CircularArc3PointClose(CGMFile container, CGMPoint p1, CGMPoint p2, CGMPoint p3, ClosureType closureType)
            :this(container)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            _closureType = closureType;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            base.ReadFromBinary(reader);

            int type = reader.ReadEnum();
            if (type == 0)
            {
                _closureType = ClosureType.PIE;
            }
            else if (type == 1)
            {
                _closureType = ClosureType.CHORD;
            }
            else
            {
                reader.Unsupported("unsupported closure type " + type);
                _closureType = ClosureType.CHORD;
            }            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            base.WriteAsBinary(writer);
            writer.WriteEnum((int)_closureType);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" ARC3PTCLOSE {WriteThreePointArcSpec()} {WriteEnum(_closureType)};");
        }

        public ClosureType Type => _closureType;
    }
}