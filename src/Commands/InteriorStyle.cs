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
            Value = (reader.ReadEnum()) switch
            {
                0 => Style.HOLLOW,
                1 => Style.SOLID,
                2 => Style.PATTERN,
                3 => Style.HATCH,
                4 => Style.EMPTY,
                5 => Style.GEOMETRIC_PATTERN,
                6 => Style.INTERPOLATED,
                _ => Style.HOLLOW,
            };
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
