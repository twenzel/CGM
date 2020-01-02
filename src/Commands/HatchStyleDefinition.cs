using System;
using System.Collections.Generic;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=18
    /// </remarks>
    public class HatchStyleDefinition : Command
    {
        public enum HatchStyle
        {
            PARALLEL = 0,
            CROSSHATCH
        }

        public int Index { get; set; }
        public HatchStyle Style { get; set; }
        public double FirstDirX { get; set; }
        public double FirstDirY { get; set; }
        public double SecondDirX { get; set; }
        public double SecondDirY { get; set; }
        public double CycleLength { get; set; }
        public List<int> GapWidths { get; set; } = new List<int>();
        public List<int> LineTypes { get; set; } = new List<int>();

        public HatchStyleDefinition(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 18, container))
        {

        }

        public HatchStyleDefinition(CGMFile container, int index, HatchStyle style, double firstX, double firstY, double secondX, double secondY, double cycleLength, int[] gapWidths, int[] lineTypes)
            : this(container)
        {
            Index = index;
            Style = style;
            FirstDirX = firstX;
            FirstDirY = firstY;
            SecondDirX = secondX;
            SecondDirY = secondY;
            CycleLength = cycleLength;
            GapWidths.AddRange(gapWidths);
            LineTypes.AddRange(lineTypes);

            if (GapWidths.Count != LineTypes.Count)
                throw new InvalidOperationException("Amount of GapWidths does not match with LineTypes!");
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Index = reader.ReadIndex();
            Style = (HatchStyle)reader.ReadEnum();
            FirstDirX = reader.ReadSizeSpecification(_container.InteriorStyleSpecificationMode);
            FirstDirY = reader.ReadSizeSpecification(_container.InteriorStyleSpecificationMode);
            SecondDirX = reader.ReadSizeSpecification(_container.InteriorStyleSpecificationMode);
            SecondDirY = reader.ReadSizeSpecification(_container.InteriorStyleSpecificationMode);
            CycleLength = reader.ReadSizeSpecification(_container.InteriorStyleSpecificationMode);

            var numberOfLines = reader.ReadInt();

            for (var i = 0; i < numberOfLines; i++)
                GapWidths.Add(reader.ReadInt());

            for (var i = 0; i < numberOfLines; i++)
                LineTypes.Add(reader.ReadInt());
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(Index);
            writer.WriteEnum((int)Style);
            writer.WriteSizeSpecification(FirstDirX, _container.InteriorStyleSpecificationMode);
            writer.WriteSizeSpecification(FirstDirY, _container.InteriorStyleSpecificationMode);
            writer.WriteSizeSpecification(SecondDirX, _container.InteriorStyleSpecificationMode);
            writer.WriteSizeSpecification(SecondDirY, _container.InteriorStyleSpecificationMode);
            writer.WriteSizeSpecification(CycleLength, _container.InteriorStyleSpecificationMode);
            writer.WriteInt(GapWidths.Count);

            if (GapWidths.Count != LineTypes.Count)
                throw new InvalidOperationException("Amount of GapWidths does not match with LineTypes!");

            foreach (var val in GapWidths)
                writer.WriteInt(val);

            foreach (var val in LineTypes)
                writer.WriteInt(val);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" HATCHSTYLEDEF {WriteIndex(Index)} {WriteEnum(Style)} {WriteVDC(FirstDirX)} {WriteVDC(FirstDirY)} {WriteVDC(SecondDirX)} {WriteVDC(SecondDirY)}");

            writer.Write($" {WriteVDC(CycleLength)} {WriteInt(GapWidths.Count)}");

            foreach (var val in GapWidths)
                writer.Write($" {WriteInt(val)}");

            foreach (var val in LineTypes)
                writer.Write($" {WriteInt(val)}");

            writer.WriteLine(";");
        }
    }
}
