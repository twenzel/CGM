using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=4
    /// </summary>
    public class LineColour : ColourCommand
    {
        public LineColour(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 4, container))
        {
        }

        public LineColour(CgmFile container, CgmColor color)
            : this(container)
        {
            SetValue(color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  linecolr {WriteColor(Color)};");
        }
    }
}
