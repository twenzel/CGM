namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=9
    /// </summary>
    public class EdgeClipping : Command
    {
        public ClippingMode Mode { get; set; }

        public EdgeClipping(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 9, container))
        {
        }

        public EdgeClipping(CgmFile container, ClippingMode mode)
            : this(container)
        {
            Mode = mode;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Mode = (ClippingMode)reader.ReadEnum();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Mode);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  EDGECLIPMODE {WriteEnum(Mode)};");
        }
    }
}