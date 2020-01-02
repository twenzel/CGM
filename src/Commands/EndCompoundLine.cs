namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=16
    /// </remarks>
    public class EndCompoundLine : Command
    {
        public EndCompoundLine(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 16, container))
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
            writer.WriteLine($" ENDCOMPOLINE;");
        }
    }
}