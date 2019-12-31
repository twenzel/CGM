using codessentials.CGM.Elements;
using System.Collections.Generic;
using System.IO;
using System;

namespace codessentials.CGM.Commands
{
    public class CharacterExpansionFactor : Command
    {
        private double _factor;

        public CharacterExpansionFactor(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 12, container))
        {
            
        }

        public CharacterExpansionFactor(CGMFile container, double factor)
            :this(container)
        {
            _factor = factor;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _factor = reader.ReadReal();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteReal(_factor);
        }

        public override string ToString()
        {
            return $"CharacterExpansionFactor {_factor}";
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" CHAREXPAN {WriteReal(_factor)};");
        }

        public double Factor => _factor;
    }
}