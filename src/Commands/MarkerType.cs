namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=6
    /// </summary>
    public class MarkerType : Command
    {
        public enum Type
        {
            DOT = 1,
            PLUS = 2,
            ASTERISK = 3,
            CIRCLE = 4,
            CROSS = 5
        }

        public Type Value { get; set; } = Type.ASTERISK;

        public MarkerType(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 6, container))
        {

        }

        public MarkerType(CGMFile container, Type type)
            : this(container)
        {
            Value = type;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var indexValue = reader.ReadIndex();
            Value = indexValue switch
            {
                1 => Type.DOT,
                2 => Type.PLUS,
                3 => Type.ASTERISK,
                4 => Type.CIRCLE,
                5 => Type.CROSS,
                _ => Type.ASTERISK,
            };
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex((int)Value);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" MARKERTYPE {WriteInt((int)Value)};");
        }

        public override string ToString()
        {
            return $"MarkerType {Value}";
        }
    }
}
