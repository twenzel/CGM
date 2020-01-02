namespace codessentials.CGM.Commands
{
    public class LineCap : CapCommand
    {
        public LineCap(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 37, container))
        {
        }

        public LineCap(CgmFile container, LineCapIndicator lineIndicator, DashCapIndicator dashIndicator)
            : this(container)
        {
            SetValues(lineIndicator, dashIndicator);
        }


        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" LINECAP {WriteInt((int)LineIndicator)} {WriteInt((int)DashIndicator)};");
        }
    }
}
