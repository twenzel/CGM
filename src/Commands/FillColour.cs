using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    public class FillColour : ColourCommand
    {
        public FillColour(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 23, container))
        {
        }


        public FillColour(CGMFile container, CGMColor color)
            :this(container)
        {
            SetValue(color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  fillcolr {WriteColor(Color)};");
        }
    }
}