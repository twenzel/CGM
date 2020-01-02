namespace codessentials.CGM.Commands
{
    public class TextPath : Command
    {
        public enum Type
        {
            RIGHT,
            LEFT,
            UP,
            DOWN
        }

        public Type Path { get; set; }

        public TextPath(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 17, container))
        {

        }

        public TextPath(CGMFile container, Type path)
            : this(container)
        {
            Path = path;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var enumValue = reader.ReadEnum();
            Path = enumValue switch
            {
                0 => Type.RIGHT,
                1 => Type.LEFT,
                2 => Type.UP,
                3 => Type.DOWN,
                _ => Type.RIGHT,
            };
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Path);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" TEXTPATH {WriteEnum(Path)};");
        }

        public override string ToString()
        {
            return $"TextPath {Path}";
        }
    }
}
