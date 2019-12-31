using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Commands
{
    public class UnknownCommand : Command
    {
        public UnknownCommand(int elementId, int elementClass, CGMFile container) 
            : this(container)
        {
            if (elementClass == 0 && elementId == 0)
                // 0, 0 is NO-OP
                return;

            // Debug.Assert(false, this.ToString());
            throw new NotSupportedException($"UnknownCommand ({this}).");

            //_messages.Add(new Message(Severity.Unimplemented, _elementClass, _elementId, "unsupported", null));
        }

        public UnknownCommand(CGMFile container)
            :base(new CommandConstructorArguments(ClassCode.ReservedForFutureUse1, 1, container))
        {
           
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            throw new NotSupportedException($"UnknownCommand ({this}) can not be read from binary.");
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            throw new NotSupportedException($"UnknownCommand ({this}) can not be exposed as binary.");
        }

        public override string ToString()
        {
            return $"Unsupported class: {_elementClass}, elementId: {_elementId}";
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            throw new NotSupportedException($"UnknownCommand ({this}) can not be exposed as clear text.");
        }
    }
}
