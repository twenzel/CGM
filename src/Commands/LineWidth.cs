﻿namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=3
    /// </summary>
    public class LineWidth : Command
    {
        public double Width { get; set; }

        public LineWidth(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 3, container))
        {

        }

        public LineWidth(CgmFile container, double width)
            : this(container)
        {
            Width = width;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Width = reader.ReadSizeSpecification(_container.LineWidthSpecificationMode);
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteSizeSpecification(Width, _container.LineWidthSpecificationMode);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  linewidth {WriteDouble(Width)};");
        }

        public override string ToString()
        {
            return $"LineWidth {Width}";
        }
    }
}
