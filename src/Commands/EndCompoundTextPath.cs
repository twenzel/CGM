namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=18
    /// </remarks>
    public class EndCompoundTextPath : Command
    {

        public EndCompoundTextPath(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 18, container))
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
            writer.WriteLine($" ENDCOMPOTEXTPATH;");
        }
    }
}