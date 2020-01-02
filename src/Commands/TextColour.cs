using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    public class TextColour : ColourCommand
    {
        public TextColour(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 14, container))
        {
        }

        public TextColour(CGMFile container, CGMColor color)
            : this(container)
        {
            SetValue(color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  textcolr {WriteColor(Color)};");
        }
    }
}
