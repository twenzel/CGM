namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=7, Element=2
    /// </remarks>
    public class ApplicationData : Command
    {
        private int _identifier;
        private string _data;

        public ApplicationData(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ExternalElements, 2, container))
        {

        }

        public ApplicationData(CGMFile container, int id, string data)
            : this(container)
        {
            _identifier = id;
            _data = data;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _identifier = reader.ReadInt();
            _data = reader.ReadString();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(_identifier);
            writer.WriteString(_data);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" APPLDATA {WriteInt(_identifier)}, {WriteString(_data)};");
        }
    }
}