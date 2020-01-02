using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=8
    /// </remarks>
    public class DeviceViewport : Command
    {
        public ViewportPoint FirstCorner { get; set; }
        public ViewportPoint SecondCorner { get; set; }

        public DeviceViewport(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 8, container))
        {

        }

        public DeviceViewport(CGMFile container, ViewportPoint firstCorner, ViewportPoint secondCorder)
            : this(container)
        {
            FirstCorner = firstCorner;
            SecondCorner = secondCorder;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            FirstCorner = reader.ReadViewportPoint();
            SecondCorner = reader.ReadViewportPoint();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteViewportPoint(FirstCorner);
            writer.WriteViewportPoint(SecondCorner);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" DEVVP {WriteViewportPoint(FirstCorner)} {WriteViewportPoint(SecondCorner)};");
        }
    }
}