namespace codessentials.CGM.Commands
{
    public class CharacterHeight : Command
    {
        public double Height { get; private set; }

        public CharacterHeight(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 15, container))
        {

        }

        public CharacterHeight(CGMFile container, double height)
            : this(container)
        {
            Height = height;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Height = reader.ReadVdc();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteVdc(Height);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  charheight {WriteDouble(Height)};");
        }

        public override string ToString()
        {
            return $"CharacterHeight {Height}";
        }
    }
}