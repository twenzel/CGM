namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=21
    /// </remarks>
    public class BeginApplicationStructure : Command
    {
        public enum InheritanceFlag
        {
            STLIST = 0,
            APS = 1
        }

        public string Id { get; private set; }
        public string Type { get; private set; }
        public InheritanceFlag Flag { get; private set; }

        public BeginApplicationStructure(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 21, container))
        {

        }

        public BeginApplicationStructure(CgmFile container, string id, string type, InheritanceFlag flag)
            : this(container)
        {
            Id = id;
            Type = type;
            Flag = flag;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Id = reader.ReadFixedString();
            Type = reader.ReadFixedString();
            Flag = (InheritanceFlag)reader.ReadEnum();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteFixedString(Id);
            writer.WriteFixedString(Type);
            writer.WriteEnum((int)Flag);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" BEGAPS {WriteString(Id)} {WriteString(Type)} {WriteEnum(Flag)};");
        }

        public override string ToString()
        {
            return $"Begin Application Structure {Id}, {Type}";
        }
    }
}