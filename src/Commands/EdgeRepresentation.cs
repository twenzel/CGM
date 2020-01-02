using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=15
    /// </remarks>
    public class EdgeRepresentation : Command
    {
        public int BundleIndex { get; set; }
        public int EdgeType { get; set; }
        public double EdgeWidth { get; set; }
        public CgmColor EdgeColor { get; set; }

        public EdgeRepresentation(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 15, container))
        {

        }

        public EdgeRepresentation(CgmFile container, int index, int type, double width, CgmColor color)
            : this(container)
        {
            BundleIndex = index;
            EdgeType = type;
            EdgeWidth = width;
            EdgeColor = color;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            BundleIndex = reader.ReadIndex();
            EdgeType = reader.ReadIndex();
            EdgeWidth = reader.ReadSizeSpecification(_container.EdgeWidthSpecificationMode);
            EdgeColor = reader.ReadColor();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(BundleIndex);
            writer.WriteIndex(EdgeType);
            writer.WriteSizeSpecification(EdgeWidth, _container.EdgeWidthSpecificationMode);
            writer.WriteColor(EdgeColor);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" EDGEREP {WriteIndex(BundleIndex)} {WriteIndex(EdgeType)} {WriteVDC(EdgeWidth)} {WriteColor(EdgeColor)};");
        }
    }
}