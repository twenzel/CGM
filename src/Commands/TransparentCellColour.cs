using System;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=20
    /// </summary>
    public class TransparentCellColour : Command
    {
        public bool Indicator { get; set; }
        public CGMColor Color { get; set; }

        public TransparentCellColour(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 20, container))
        {
           
        }

        public TransparentCellColour(CGMFile container, bool indicator, CGMColor color)
            :this(container)
        {
            Indicator = indicator;
            Color = color;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Indicator = reader.ReadBool();
            Color = reader.ReadColor();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteBool(Indicator);
            writer.WriteColor(Color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" TRANSPCELLCOLR {WriteBool(Indicator)} {WriteColor(Color)};");
        }
    }
}