using System;

namespace codessentials.CGM.Commands
{
    public class UnknownCommand : Command
    {
        public UnknownCommand(int elementId, int elementClass, CgmFile container)
            : this(container)
        {
            if (elementClass == 0 && elementId == 0)
                // 0, 0 is NO-OP
                return;

            throw new NotSupportedException($"UnknownCommand ({this}).");
        }

        public UnknownCommand(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.ReservedForFutureUse1, 1, container))
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
