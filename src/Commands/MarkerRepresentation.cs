using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=12
    /// </remarks>
    public class MarkerRepresentation : Command
    {
        public int Index { get; set; }
        public int Type { get; set; }
        public double Size { get; set; }
        public CgmColor Color { get; set; }

        public MarkerRepresentation(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 12, container))
        {

        }

        public MarkerRepresentation(CgmFile container, int index, int type, double size, CgmColor color)
            : this(container)
        {
            Index = index;
            Type = type;
            Size = size;
            Color = color;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Index = reader.ReadIndex();
            Type = reader.ReadIndex();
            Size = reader.ReadSizeSpecification(_container.MarkerSizeSpecificationMode);
            Color = reader.ReadColor();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(Index);
            writer.WriteIndex(Type);
            writer.WriteSizeSpecification(Size, _container.MarkerSizeSpecificationMode);
            writer.WriteColor(Color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" MARKERREP {WriteIndex(Index)} {WriteIndex(Type)} {WriteVDC(Size)} {WriteColor(Color)};");
        }
    }
}
