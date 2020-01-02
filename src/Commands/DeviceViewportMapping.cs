namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=10
    /// </remarks>
    public class DeviceViewportMapping : Command
    {
        public enum Isotropy
        {
            NOTFORCED = 0,
            FORCED
        }

        public enum Horizontalalignment
        {
            LEFT = 0,
            CTR,
            RIGHT
        }

        public enum Verticalalignment
        {
            BOTTOM = 0,
            CTR,
            TOP
        }

        public Isotropy IsotropyValue { get; set; }

        public Horizontalalignment HorizontalAlignment { get; set; }

        public Verticalalignment VerticalAlignment { get; set; }

        public DeviceViewportMapping(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 10, container))
        {

        }

        public DeviceViewportMapping(CGMFile container, Isotropy isotropy, Horizontalalignment horzAlignment, Verticalalignment vertAlignment)
            : this(container)
        {
            IsotropyValue = isotropy;
            HorizontalAlignment = horzAlignment;
            VerticalAlignment = vertAlignment;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            IsotropyValue = (Isotropy)reader.ReadEnum();
            HorizontalAlignment = (Horizontalalignment)reader.ReadEnum();
            VerticalAlignment = (Verticalalignment)reader.ReadEnum();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)IsotropyValue);
            writer.WriteEnum((int)HorizontalAlignment);
            writer.WriteEnum((int)VerticalAlignment);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" DEVVPMAP {WriteEnum(IsotropyValue)} {WriteEnum(HorizontalAlignment)} {WriteEnum(VerticalAlignment)};");
        }
    }
}