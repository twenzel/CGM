namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=36
    /// </summary>
    public class PickIdentifier : Command
    {
        public int Identifier { get; set; }

        public PickIdentifier(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 36, container))
        {

        }

        public PickIdentifier(CgmFile container, int id)
            : this(container)
        {
            Identifier = id;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Identifier = reader.ReadName();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteName(Identifier);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" PICKID {WriteName(Identifier)};");
        }
    }
}
