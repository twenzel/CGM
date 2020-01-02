namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=40
    /// </summary>
    public class LineTypeInitialOffset : Command
    {
        public double Offset { get; set; }

        public LineTypeInitialOffset(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 40, container))
        {

        }

        public LineTypeInitialOffset(CgmFile container, double offset)
            : this(container)
        {
            Offset = offset;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Offset = reader.ReadReal();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteReal(Offset);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" LINETYPEINITOFFSET {WriteReal(Offset)};");
        }
    }
}
