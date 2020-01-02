namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=8, ElementId=7
    /// </summary>
    public class SegmentPickPriority : Command
    {
        public int Identifier { get; set; }
        public int Prio { get; set; }

        public SegmentPickPriority(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.SegmentControlandSegmentAttributeElements, 7, container))
        {

        }

        public SegmentPickPriority(CGMFile container, int id, int prio)
            : this(container)
        {
            Identifier = id;
            Prio = prio;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Identifier = reader.ReadName();
            Prio = reader.ReadInt();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteName(Identifier);
            writer.WriteInt(Prio);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" SEGPICKPRI {WriteName(Identifier)} {WriteInt(Prio)};");
        }
    }
}
