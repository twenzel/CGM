using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=33
    /// </summary>
    public class PatternSize : Command
    {
        public double HeightX { get; set; }
        public double HeightY { get; set; }
        public double WidthX { get; set; }
        public double WidthY { get; set; }

        public PatternSize(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 33, container))
        {
            
        }

        public PatternSize(CGMFile container, double heightX, double heightY, double widthX, double widthY)
            :this(container)
        {
            HeightX = heightX;
            HeightY = heightY;
            WidthX = widthX;
            WidthY = widthY;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            HeightX = reader.ReadSizeSpecification(_container.InteriorStyleSpecificationMode);
            HeightY = reader.ReadSizeSpecification(_container.InteriorStyleSpecificationMode);
            WidthX = reader.ReadSizeSpecification(_container.InteriorStyleSpecificationMode);
            WidthY = reader.ReadSizeSpecification(_container.InteriorStyleSpecificationMode);            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteSizeSpecification(HeightX, _container.InteriorStyleSpecificationMode);
            writer.WriteSizeSpecification(HeightY, _container.InteriorStyleSpecificationMode);
            writer.WriteSizeSpecification(WidthX, _container.InteriorStyleSpecificationMode);
            writer.WriteSizeSpecification(WidthY, _container.InteriorStyleSpecificationMode);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" PATSIZE {WriteVDC(HeightX)} {WriteVDC(HeightY)} {WriteVDC(WidthX)} {WriteVDC(WidthY)};");
        }
    }
}