namespace codessentials.CGM.Commands
{
    public class Escape : Command
    {
        public int Identifier { get; set; }
        public string DataRecord { get; set; }

        public Escape(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.EscapeElement, 0, container))
        {

        }

        public Escape(CGMFile container, int id, string record)
            : this(container)
        {
            Identifier = id;
            DataRecord = record;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Identifier = reader.ReadInt();
            DataRecord = reader.ReadString();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(Identifier);
            writer.WriteString(DataRecord);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" ESCAPE {WriteInt(Identifier)} {WriteString(DataRecord)};");
        }

        public override string ToString()
        {
            return $"Escape identifer={Identifier}";
        }
    }
}