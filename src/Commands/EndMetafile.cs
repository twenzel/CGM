namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=2
    /// </remarks>
    public class EndMetafile : Command
    {
        public EndMetafile(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 2, container))
        {
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {

        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            // no parameter
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"ENDMF;");
        }

        public override string ToString()
        {
            return "EndMetafile";
        }
    }
}