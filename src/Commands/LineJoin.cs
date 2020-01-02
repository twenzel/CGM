namespace codessentials.CGM.Commands
{
    public class LineJoin : JoinCommand
    {
        public LineJoin(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 38, container))
        {
        }

        public LineJoin(CGMFile container, JoinIndicator type)
            : this(container)
        {
            SetValue(type);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" LINEJOIN {WriteInt((int)Type)};");
        }
    }
}
