namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=1
    /// </remarks>
    public class BeginMetafile : Command
    {
        public string FileName { get; private set; }

        public BeginMetafile(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 1, container))
        {

        }

        public BeginMetafile(CGMFile container, string fileName)
            : this(container)
        {
            FileName = fileName;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            FileName = reader.ArgumentsCount > 0 ? reader.ReadString() : "";
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteString(FileName);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"BEGMF {WriteString(FileName)};");
        }

        public override string ToString()
        {
            return "BeginMetafile " + FileName;
        }
    }
}