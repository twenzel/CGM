namespace codessentials.CGM.Commands
{
    public class EdgeType : DashTypeCommand
    {
        public EdgeType(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 27, container))
        {
        }

        public EdgeType(CGMFile container, DashType type)
            : this(container)
        {
            SetValue(type);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  edgetype {WriteDashType()};");
        }
    }
}