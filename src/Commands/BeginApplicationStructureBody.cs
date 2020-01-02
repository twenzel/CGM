namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, Element=22
    /// </remarks>
    public class BeginApplicationStructureBody : Command
    {
        public BeginApplicationStructureBody(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 22, container))
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
            writer.WriteLine($" BEGAPSBODY;");
        }
    }
}