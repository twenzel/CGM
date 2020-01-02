namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=8, ElementId=5
    /// </summary>
    public class SegmentHighlighting : Command
    {
        public enum Highlighting
        {
            NORMAL,
            HIGHL
        }

        public int Identifier { get; set; }
        public Highlighting Value { get; set; }

        public SegmentHighlighting(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.SegmentControlandSegmentAttributeElements, 5, container))
        {

        }

        public SegmentHighlighting(CGMFile container, int id, Highlighting value)
            : this(container)
        {
            Identifier = id;
            Value = value;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Identifier = reader.ReadName();
            Value = (Highlighting)reader.ReadEnum();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteName(Identifier);
            writer.WriteEnum((int)Value);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" SEGHIGHL {WriteName(Identifier)} {WriteEnum(Value)};");
        }
    }
}
