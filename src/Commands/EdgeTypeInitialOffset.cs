namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=47
    /// </summary>
    public class EdgeTypeInitialOffset : Command
    {
        public double Offset { get; set; }

        public EdgeTypeInitialOffset(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 47, container))
        {

        }

        public EdgeTypeInitialOffset(CGMFile container, double offset)
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
            writer.WriteLine($" EDGETYPEINITOFFSET {WriteReal(Offset)};");
        }
    }
}