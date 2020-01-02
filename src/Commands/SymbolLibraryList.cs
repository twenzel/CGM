using System.Collections.Generic;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=1, Element=23
    /// </summary>
    public class SymbolLibraryList : Command
    {
        public List<string> Names { get; set; } = new List<string>();

        public SymbolLibraryList(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 23, container))
        {

        }

        public SymbolLibraryList(CgmFile container, string[] names)
            : this(container)
        {
            Names.AddRange(names);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            while (reader.CurrentArg < reader.ArgumentsCount)
                Names.Add(reader.ReadFixedString());
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach (var name in Names)
                writer.WriteFixedString(name);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write(" SYMBOLLIBLIST");

            foreach (var s in Names)
                writer.Write(" " + WriteString(s));

            writer.WriteLine(";");
        }
    }
}
