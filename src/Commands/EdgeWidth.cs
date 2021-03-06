﻿namespace codessentials.CGM.Commands
{
    public class EdgeWidth : Command
    {
        public double Width { get; set; }

        public EdgeWidth(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 28, container))
        {

        }

        public EdgeWidth(CgmFile container, double width)
            : this(container)
        {
            Width = width;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Width = reader.ReadSizeSpecification(_container.EdgeWidthSpecificationMode);
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteSizeSpecification(Width, _container.EdgeWidthSpecificationMode);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  EDGEWIDTH {WriteDouble(Width)};");
        }
    }
}