namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=0
    /// </remarks>
    public class NoOp : Command
    {
        public NoOp(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 0, container))
        {

        }

        public override void ReadFromBinary(IBinaryReader reader)
        {

        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {

        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            // nothing to write
        }

        public override string ToString()
        {
            return "NoOp";
        }
    }
}
