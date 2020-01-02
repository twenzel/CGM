namespace codessentials.CGM.Commands
{
    public class InteriorStyle : Command
    {
        public enum Style
        {
            HOLLOW,
            SOLID,
            PATTERN,
            HATCH,
            EMPTY,
            GEOMETRIC_PATTERN,
            INTERPOLATED
        }

        public Style Value { get; set; }

        public InteriorStyle(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 22, container))
        {

        }

        public InteriorStyle(CGMFile container, Style style)
            : this(container)
        {
            Value = style;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            switch (reader.ReadEnum())
            {
                case 0:
                    Value = Style.HOLLOW;
                    break;
                case 1:
                    Value = Style.SOLID;
                    break;
                case 2:
                    Value = Style.PATTERN;
                    break;
                case 3:
                    Value = Style.HATCH;
                    break;
                case 4:
                    Value = Style.EMPTY;
                    break;
                case 5:
                    Value = Style.GEOMETRIC_PATTERN;
                    break;
                case 6:
                    Value = Style.INTERPOLATED;
                    break;
                default:
                    Value = Style.HOLLOW;
                    break;
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Value);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  intstyle {WriteEnum(Value)};");
        }
    }
}
