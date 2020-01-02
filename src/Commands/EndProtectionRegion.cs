namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=14
    /// </remarks>
    public class EndProtectionRegion : Command
    {
        public EndProtectionRegion(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 14, container))
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
            writer.WriteLine($" ENDPROTREGION;");
        }
    }
}