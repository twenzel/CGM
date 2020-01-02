namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=9
    /// </remarks>
    public class MaximumColourIndex : Command
    {
        public int Value { get; set; }

        public MaximumColourIndex(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 9, container))
        {

        }

        public MaximumColourIndex(CGMFile container, int index)
            : this(container)
        {
            Value = index;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Value = reader.ReadColorIndex();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteColorIndex(Value);
        }

        public override string ToString()
        {
            return $"MaximumColourIndex {Value}";
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" maxcolrindex {Value};");
        }
    }
}
