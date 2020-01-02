using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=8
    /// </summary>
    public class MarkerColour : ColourCommand
    {
        public MarkerColour(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 8, container))
        {
        }

        public MarkerColour(CgmFile container, CgmColor color)
            : this(container)
        {
            SetValue(color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" MARKERCOLR {WriteColor(Color)};");
        }
    }
}
