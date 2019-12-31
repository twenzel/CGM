using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=46
    /// </summary>
    public class EdgeTypeContinuation : Command
    {
        public int Mode { get; set; }

        public EdgeTypeContinuation(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 46, container))
        {           
        }

        public EdgeTypeContinuation(CGMFile container, int mode)
            :this(container)
        {
            Mode = mode;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Mode = reader.ReadIndex();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(Mode);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" EDGETYPECONT {WriteIndex(Mode)};");
        }
    }
}