namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=15
    /// </remarks>
    public class BeginCompoundLine : Command
    {
        public BeginCompoundLine(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 15, container))
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
            writer.WriteLine($" BEGCOMPOLINE;");
        }
    }
}