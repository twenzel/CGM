namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=39
    /// </summary>
    public class LineTypeContinuation : Command
    {
        public int Mode { get; set; }

        public LineTypeContinuation(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 39, container))
        {
        }

        public LineTypeContinuation(CGMFile container, int mode)
            : this(container)
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
            writer.WriteLine($" LINETYPECONT {WriteIndex(Mode)};");
        }
    }
}
