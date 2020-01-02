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
            switch (enumValue)
            {
                case 0:
                    Path = Type.RIGHT;
                    break;
                case 1:
                    Path = Type.LEFT;
                    break;
                case 2:
                    Path = Type.UP;
                    break;
                case 3:
                    Path = Type.DOWN;
                    break;
                default:
                    Path = Type.RIGHT;
                    break;
            }
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
