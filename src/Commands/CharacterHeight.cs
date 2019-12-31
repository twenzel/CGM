using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    public class CharacterHeight : Command
    {
        private double _characterHeight;

        public CharacterHeight(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 15, container))
        {
            
        }

        public CharacterHeight(CGMFile container, double height)
            :this(container)
        {
            _characterHeight = height;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _characterHeight = reader.ReadVdc();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteVdc(_characterHeight);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  charheight {WriteDouble(_characterHeight)};");
        }

        public override string ToString()
        {
            return $"CharacterHeight {_characterHeight}";
        }

        public double Height
        {
            get { return _characterHeight; }
        }
    }
}