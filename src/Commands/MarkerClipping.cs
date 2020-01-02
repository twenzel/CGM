namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// ClassId=3, ElementId=8
    /// </remarks>
    public class MarkerClipping : Command
    {
        public ClippingMode Mode { get; set; }

        public MarkerClipping(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 8, container))
        {

        }

        public MarkerClipping(CGMFile container, ClippingMode mode)
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
            writer.WriteLine($"  MARKERCLIPMODE {WriteEnum(Mode)};");
        }
    }
}
