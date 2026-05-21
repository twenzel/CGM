using System.Collections.Generic;
using System.Linq;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=12
    /// </remarks>
    public class MetafileDefaultsReplacement : Command
    {
        public List<Command> EmbeddedCommands { get; set; } = new List<Command>();

        public MetafileDefaultsReplacement(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 12, container))
        {

        }

        public MetafileDefaultsReplacement(CgmFile container, List<Command> commands)
            : this(container)
        {
            EmbeddedCommands = commands;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            EmbeddedCommands = reader.ReadEmbeddedCommands();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEmbeddedCommands(EmbeddedCommands);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" BEGMFDEFAULTS;");
            EmbeddedCommands.ForEach(e => e.WriteAsClearText(writer));
            writer.WriteLine($"  ENDMFDEFAULTS ;");
        }

        public override string ToString()
        {
            return "MetafileDefaultsReplacement " + string.Join(" ", EmbeddedCommands.Select(e => e.ToString()));
        }
    }
}
