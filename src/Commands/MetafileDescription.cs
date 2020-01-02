namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=2
    /// </remarks>
    public class MetafileDescription : Command
    {
        public string Description { get; private set; }

        public MetafileDescription(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 2, container))
        {

        }

        public MetafileDescription(CGMFile container, string description)
            : this(container)
        {
            Description = description;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Description = reader.ReadFixedStringWithFallback(reader.Arguments.Length);
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteFixedString(Description);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" mfdesc {WriteString(Description)};");
        }

        public override string ToString()
        {
            return $"MetafileDescription {Description}";
        }
    }
}
