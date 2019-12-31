using System;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=11
    /// </remarks>
    public class LineRepresentation : Command
    {
        public int Index { get; set; }
        public int LineType { get; set; }
        public double LineWidth { get; set; }
        public CGMColor Color { get; set; }

        public LineRepresentation(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 11, container))
        {
            
        }

        public LineRepresentation(CGMFile container, int index, int lineType, double width, CGMColor color)
            :this(container)
        {
            Index = index;
            LineType = lineType;
            LineWidth = width;
            Color = color;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Index = reader.ReadIndex();
            LineType = reader.ReadIndex();
            LineWidth = reader.ReadSizeSpecification(_container.LineWidthSpecificationMode);
            Color = reader.ReadColor();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(Index);
            writer.WriteIndex(LineType);
            writer.WriteSizeSpecification(LineWidth, _container.LineWidthSpecificationMode);
            writer.WriteColor(Color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" LINEREP {WriteIndex(Index)} {WriteIndex(LineType)} {WriteVDC(LineWidth)} {WriteColor(Color)};");
        }
    }
}