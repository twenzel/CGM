using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    public class CharacterSpacing : Command
    {
        private double _additionalInterCharacterSpace;

        public CharacterSpacing(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 13, container))
        {
            
        }

        public CharacterSpacing(CGMFile container, double space)
            :this(container)
        {
            _additionalInterCharacterSpace = space;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _additionalInterCharacterSpace = reader.ReadReal();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteReal(_additionalInterCharacterSpace);
        }

        public override string ToString()
        {
            return $"CharacterSpacing {_additionalInterCharacterSpace}";
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" CHARSPACE {WriteReal(_additionalInterCharacterSpace)};");
        }

        public double Space => _additionalInterCharacterSpace;
    }
}