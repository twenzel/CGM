namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=7
    /// </summary>
    public class MarkerSize : Command
    {
        public double Width { get; set; }

        public MarkerSize(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 7, container))
        {

        }

        public MarkerSize(CGMFile container, double width)
            : this(container)
        {
            Width = width;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Width = reader.ReadSizeSpecification(_container.MarkerSizeSpecificationMode);
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteSizeSpecification(Width, _container.MarkerSizeSpecificationMode);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" MARKERSIZE {WriteVDC(Width)};");
        }

        public override string ToString()
        {
            return $"MarkerSize {Width}";
        }
    }
}
