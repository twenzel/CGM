using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=3
    /// </summary>
    public class AuxiliaryColour : ColourCommand
    {
        public AuxiliaryColour(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 3, container))
        {

        }

        public AuxiliaryColour(CgmFile container, CgmColor color)
            : this(container)
        {
            SetValue(color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" AUXCOLR {WriteColor(Color)};");
        }
    }
}