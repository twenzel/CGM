namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=4
    /// </summary>
    public class Transparency : Command
    {
        public bool Flag { get; set; }

        public Transparency(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 4, container))
        {

        }

        public Transparency(CGMFile container, bool flag)
            : this(container)
        {
            Flag = flag;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Flag = reader.ReadBool();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteBool(Flag);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  transparency {WriteBool(Flag)};");
        }
    }
}
