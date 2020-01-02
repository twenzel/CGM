namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=2, Element=4
    /// </summary>
    public class MarkerSizeSpecificationMode : Command
    {
        public SpecificationMode Mode { get; set; }

        public MarkerSizeSpecificationMode(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 4, container))
        {

        }

        public MarkerSizeSpecificationMode(CgmFile container, SpecificationMode mode)
            : this(container)
        {
            Mode = mode;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var mode = reader.ReadEnum();
            Mode = SpecificationModeTools.GetMode(mode);
            _container.MarkerSizeSpecificationMode = Mode;
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Mode);
            _container.MarkerSizeSpecificationMode = Mode;
        }

        public override string ToString()
        {
            return $"MarkerSizeSpecificationMode {Mode}";
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" MARKERSIZEMODE  {WriteEnum(Mode)};");
        }
    }
}
