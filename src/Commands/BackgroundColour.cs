using System.Drawing;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=7
    /// </remarks>
    public class BackgroundColour : Command
    {
        /// <summary>
        /// Gets the background color
        /// </summary>
        public Color Color { get; private set; }

        public BackgroundColour(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 7, container))
        {

        }

        public BackgroundColour(CgmFile container, Color color)
            : this(container)
        {
            Color = color;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Color = reader.ReadDirectColor();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteDirectColor(Color);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  backcolr {WriteColor(Color, _container.ColourModel)};");
        }

        public override string ToString()
        {
            return $"BackgroundColour {Color}";
        }
    }
}