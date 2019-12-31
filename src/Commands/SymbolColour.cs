using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=49
    /// </summary>
    public class SymbolColour : ColourCommand
    {    
        public SymbolColour(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 49, container))
        {
           
        }

        public SymbolColour(CGMFile container, CGMColor color)
            :this(container)
        {
            SetValue(color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" SYMBOLCOLR {WriteColor(Color)};");
        }
    }
}