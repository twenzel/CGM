using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=7
    /// </remarks>
    public class BackgroundColour : Command
    {
        private Color _backgroundColor;

        public BackgroundColour(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 7, container))
        {
           
        }

        public BackgroundColour(CGMFile container, Color color)
            :this(container)
        {
            _backgroundColor = color;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _backgroundColor = reader.ReadDirectColor();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteDirectColor(_backgroundColor);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  backcolr {WriteColor(_backgroundColor, _container.ColourModel)};");
        }

        public override string ToString()
        {
            return $"BackgroundColour {_backgroundColor}";
        }

        public Color Color => _backgroundColor;
    }
}