using codessentials.CGM.Export;
using System.Collections.Generic;
using System.IO;
using System;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=2
    /// </remarks>
    public class MetafileDescription : Command
    {
        private string _description;

        public string Description
        {
            get { return _description; }
        }

        public MetafileDescription(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 2, container))
        {
           
        }

        public MetafileDescription(CGMFile container, string description)
            :this(container)
        {
            _description = description;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _description = reader.ReadFixedStringWithFallback(reader.Arguments.Length);            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteFixedString(_description);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" mfdesc {WriteString(_description)};");
        }

        public override string ToString()
        {
            return $"MetafileDescription {_description}";
        }
    }
}