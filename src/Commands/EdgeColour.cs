using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    public class EdgeColour : ColourCommand
    {
        public EdgeColour(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 29, container))
        {

        }

        public EdgeColour(CGMFile container, CGMColor color)
            : this(container)
        {
            SetValue(color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  EDGECOLR {WriteColor(Color)};");
        }
    }
}