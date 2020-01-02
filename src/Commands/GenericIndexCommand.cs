namespace codessentials.CGM.Commands
{
    public abstract class GenericIndexCommand : Command
    {
        public int Index { get; set; }
        public string Name { get; }

        protected GenericIndexCommand(CommandConstructorArguments arguments, string name)
            : base(arguments)
        {
            Name = name;
        }

        protected void SetValue(int index)
        {
            Index = index;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Index = reader.ReadIndex();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(Index);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" {Name} {WriteIndex(Index)};");
        }

        public override string ToString()
        {
            return $"{Name} {Index};";
        }
    }
}
