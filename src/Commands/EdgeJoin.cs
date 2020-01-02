namespace codessentials.CGM.Commands
{
    public class EdgeJoin : JoinCommand
    {
        public EdgeJoin(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 45, container))
        {
        }

        public EdgeJoin(CgmFile container, JoinIndicator type)
            : this(container)
        {
            SetValue(type);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" EDGEJOIN {WriteInt((int)Type)};");
        }
    }
}