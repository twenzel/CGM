namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=23
    /// </remarks>
    public class EndApplicationStructure : Command
    {
        public EndApplicationStructure(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 23, container))
        {

        }

        public override void ReadFromBinary(IBinaryReader reader)
        {

        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {

        }

        public override string ToString()
        {
            return "EndApplicationStructure";
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" ENDAPS;");
        }
    }
}