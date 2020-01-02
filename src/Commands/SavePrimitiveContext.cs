namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=11
    /// </summary>
    public class SavePrimitiveContext : Command
    {
        public int Name { get; set; }

        public SavePrimitiveContext(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 11, container))
        {

        }

        public SavePrimitiveContext(CGMFile container, int name)
            : this(container)
        {
            Name = name;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Name = reader.ReadName();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteName(Name);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" SAVEPRIMCONT {WriteName(Name)};");
        }
    }
}
