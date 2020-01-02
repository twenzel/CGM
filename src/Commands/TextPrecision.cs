namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=11
    /// </summary>
    public class TextPrecision : Command
    {
        public TextPrecisionType Value { get; set; }

        public TextPrecision(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 11, container))
        {

        }

        public TextPrecision(CGMFile container, TextPrecisionType value)
            : this(container)
        {
            Value = value;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var enumValue = reader.ReadEnum();
            switch (enumValue)
            {
                case 0:
                    Value = TextPrecisionType.STRING;
                    break;
                case 1:
                    Value = TextPrecisionType.CHAR;
                    break;
                case 2:
                    Value = TextPrecisionType.STROKE;
                    break;
                default:
                    Value = TextPrecisionType.STRING;
                    reader.Unsupported("unsupported text precision " + enumValue);
                    break;
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Value);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" TEXTPREC {WriteEnum(Value)};");
        }

        public override string ToString()
        {
            return $"TextPrecision {Value}";
        }
    }

    public enum TextPrecisionType
    {
        STRING,
        CHAR,
        STROKE
    }
}
