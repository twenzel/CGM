namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=12
    /// </summary>
    public class RestorePrimitiveContext : Command
    {
        public int Name { get; set; }

        public RestorePrimitiveContext(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 12, container))
        {

        }

        public RestorePrimitiveContext(CGMFile container, int name)
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
            writer.WriteLine($" RESPRIMCONT {WriteName(Name)};");
        }
    }
}
