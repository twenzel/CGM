using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=16
    /// </summary>
    public class CircularArcCentreClose : CircularArcCentre
    {
        private ClosureType _closureType;

        public CircularArcCentreClose(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 16, container))
        {
           
        }

        public CircularArcCentreClose(CGMFile container, CGMPoint center, double startDeltaX, double startDeltaY, double endDeltaX, double endDeltaY, double radius, ClosureType closure)
            :this(container)
        {
            SetValues(center, startDeltaX, startDeltaY, endDeltaX, endDeltaY, radius);
            _closureType = closure;
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
            writer.Write("  ARCCTRCLOSE ");
            WriteValues(writer);
            writer.Write($" {WriteEnum(_closureType)}");
            writer.WriteLine(";");
        }

        public ClosureType Type => _closureType;
    }
}