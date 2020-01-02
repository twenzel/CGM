using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=14
    /// </remarks>
    public class FillRepresentation : Command
    {
        public int BundleIndex { get; set; }
        public InteriorStyle.Style Style { get; set; }
        public CGMColor Color { get; set; }
        public int HatchIndex { get; set; }
        public int PatternIndex { get; set; }

        public FillRepresentation(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 14, container))
        {

        }

        public FillRepresentation(CGMFile container, int index, InteriorStyle.Style style, CGMColor color, int hatchIndex, int patternIndex)
            : this(container)
        {
            BundleIndex = index;
            Style = style;
            Color = color;
            HatchIndex = hatchIndex;
            PatternIndex = patternIndex;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            BundleIndex = reader.ReadIndex();
            Style = (InteriorStyle.Style)reader.ReadEnum();
            Color = reader.ReadColor();
            HatchIndex = reader.ReadIndex();
            PatternIndex = reader.ReadIndex();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(BundleIndex);
            writer.WriteEnum((int)Style);
            writer.WriteColor(Color);
            writer.WriteIndex(HatchIndex);
            writer.WriteIndex(PatternIndex);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" FILLREP {WriteIndex(BundleIndex)} {WriteEnum(Style)} {WriteColor(Color)} {WriteIndex(HatchIndex)} {WriteIndex(PatternIndex)};");
        }
    }
}