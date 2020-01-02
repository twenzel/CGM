namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=10
    /// </summary>
    public class NewRegion : Command
    {
        public NewRegion(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 10, container))
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
            writer.WriteLine(" NEWREGION;");
        }
    }
}
