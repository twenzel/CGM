namespace codessentials.CGM.Commands
{
    public class EdgeCap : CapCommand
    {
        public EdgeCap(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 44, container))
        {
        }

        public EdgeCap(CGMFile container, LineCapIndicator lineIndicator, DashCapIndicator dashIndicator)
            : this(container)
        {
            SetValues(lineIndicator, dashIndicator);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" EDGECAP {WriteInt((int)LineIndicator)} {WriteInt((int)DashIndicator)};");
        }
    }
}