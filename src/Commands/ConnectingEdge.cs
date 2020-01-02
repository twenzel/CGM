namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=21
    /// </summary>
    public class ConnectingEdge : Command
    {
        public ConnectingEdge(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 21, container))
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
            writer.WriteLine(" CONNEDGE;");
        }
    }
}