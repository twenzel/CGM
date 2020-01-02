namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=6
    /// </remarks>
    public class BeginSegment : Command
    {
        public int Id { get; private set; }

        public BeginSegment(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 6, container))
        {

        }

        public BeginSegment(CgmFile container, int id)
            : this(container)
        {
            Id = id;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Id = reader.ReadName();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteName(Id);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" BEGSEG {WriteName(Id)};");
        }
    }
}