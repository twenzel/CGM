namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=7, Element=2
    /// </remarks>
    public class ApplicationData : Command
    {
        public int Identifier { get; set; }
        public string Data { get; set; }

        public ApplicationData(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.ExternalElements, 2, container))
        {

        }

        public ApplicationData(CgmFile container, int id, string data)
            : this(container)
        {
            Identifier = id;
            Data = data;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Identifier = reader.ReadInt();
            Data = reader.ReadString();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(Identifier);
            writer.WriteString(Data);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" APPLDATA {WriteInt(Identifier)}, {WriteString(Data)};");
        }
    }
}
