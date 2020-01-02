namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=2, Element=3
    /// </summary>
    public class LineWidthSpecificationMode : Command
    {
        public SpecificationMode Mode { get; set; }

        public LineWidthSpecificationMode(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 3, container))
        {

        }

        public LineWidthSpecificationMode(CgmFile container, SpecificationMode mode)
            : this(container)
        {
            Mode = mode;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var mode = reader.ReadEnum();
            Mode = SpecificationModeTools.GetMode(mode);
            _container.LineWidthSpecificationMode = Mode;
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Mode);
            _container.LineWidthSpecificationMode = Mode;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  linewidthmode {WriteEnum(Mode)};");
        }

        public override string ToString()
        {
            return $"LineWidthSpecificationMode {Mode}";
        }
    }
}
