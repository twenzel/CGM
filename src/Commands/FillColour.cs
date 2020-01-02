using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    public class FillColour : ColourCommand
    {
        public FillColour(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 23, container))
        {
        }


        public FillColour(CgmFile container, CgmColor color)
            : this(container)
        {
            SetValue(color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  fillcolr {WriteColor(Color)};");
        }
    }
}