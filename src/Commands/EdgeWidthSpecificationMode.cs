namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=2, Element=5
    /// </summary>
    public class EdgeWidthSpecificationMode : Command
    {
        public SpecificationMode Mode { get; set; }

        public EdgeWidthSpecificationMode(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 5, container))
        {

        }

        public EdgeWidthSpecificationMode(CGMFile container, SpecificationMode mode)
            : this(container)
        {
            Mode = mode;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var mode = reader.ReadEnum();
            Mode = SpecificationModeTools.GetMode(mode);
            _container.EdgeWidthSpecificationMode = Mode;
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Mode);
            _container.EdgeWidthSpecificationMode = Mode;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  edgewidthmode {Mode.ToString().ToLower()};");
        }

        public override string ToString()
        {
            return $"EdgeWidthSpecificationMode {Mode}";
        }
    }
}