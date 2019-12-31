using codessentials.CGM.Import;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=12
    /// </remarks>
    public class MetafileDefaultsReplacement : Command
    {
        public Command EmbeddedCommand { get; set; }

        public MetafileDefaultsReplacement(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 12, container))
        {
            
        }

        public MetafileDefaultsReplacement(CGMFile container, Command command)
            :this(container)
        {
            EmbeddedCommand = command;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            EmbeddedCommand = reader.ReadEmbeddedCommand();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {            
            writer.WriteEmbeddedCommand(EmbeddedCommand);            
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" BEGMFDEFAULTS;");
            EmbeddedCommand.WriteAsClearText(writer);
            writer.WriteLine($"  ENDMFDEFAULTS ;");
        }

        public override string ToString()
        {
            return "MetafileDefaultsReplacement " + EmbeddedCommand.ToString();
        }
    }
}