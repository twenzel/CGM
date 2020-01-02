namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// ClassId=3, ElementId=7
    /// </remarks>
    public class LineClipping : Command
    {
        public ClippingMode Mode { get; set; }

        public LineClipping(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 7, container))
        {

        }

        public LineClipping(CGMFile container, ClippingMode mode)
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
            writer.WriteLine($"  LINECLIPMODE {WriteEnum(Mode)};");
        }
    }
}
