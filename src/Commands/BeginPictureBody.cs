namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=4
    /// </remarks>
    public class BeginPictureBody : Command
    {
        public BeginPictureBody(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 4, container))
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
            writer.WriteLine($" BEGPICBODY;");
        }
    }
}