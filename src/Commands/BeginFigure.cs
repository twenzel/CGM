namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=8
    /// </remarks>
    public class BeginFigure : Command
    {
        public BeginFigure(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 8, container))
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
            writer.WriteLine($" BEGFIGURE;");
        }
    }
}