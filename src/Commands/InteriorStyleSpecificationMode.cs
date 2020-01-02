namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=16
    /// </remarks>
    public class InteriorStyleSpecificationMode : Command
    {
        public SpecificationMode Mode { get; set; }

        public InteriorStyleSpecificationMode(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 16, container))
        {

        }

        public InteriorStyleSpecificationMode(CGMFile container, SpecificationMode mode)
            : this(container)
        {
            Mode = mode;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var enumValue = reader.ReadEnum();
            switch (enumValue)
            {
                case 0:
                    Mode = SpecificationMode.ABS;
                    break;
                case 1:
                    Mode = SpecificationMode.SCALED;
                    break;
                case 2:
                    Mode = SpecificationMode.FRACTIONAL;
                    break;
                case 3:
                    Mode = SpecificationMode.MM;
                    break;
                default:
                    Mode = SpecificationMode.ABS;
                    reader.Unsupported("unsupported specification mode " + enumValue);
                    break;
            }

            _container.InteriorStyleSpecificationMode = Mode;
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Mode);
            _container.InteriorStyleSpecificationMode = Mode;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" INTSTYLEMODE {WriteEnum(Mode)};");
        }

        public override string ToString()
        {
            return $"InteriorStyleSpecificationMode mode={Mode}";
        }
    }
}
