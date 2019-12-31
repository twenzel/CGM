using codessentials.CGM.Classes;
using System.Drawing;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=3
    /// </summary>
    public class AuxiliaryColour : ColourCommand
    {  
        public AuxiliaryColour(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 3, container))
        {
           
        }

        public AuxiliaryColour(CGMFile container, CGMColor color)
            :this(container)
        {
            SetValue(color);
        }
      
        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" AUXCOLR {WriteColor(Color)};");
        }
    }
}