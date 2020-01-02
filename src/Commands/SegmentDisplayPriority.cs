namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=8, ElementId=6
    /// </summary>
    public class SegmentDisplayPriority : Command
    {
        public int Name { get; set; }
        public int Prio { get; set; }

        public SegmentDisplayPriority(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.SegmentControlandSegmentAttributeElements, 6, container))
        {

        }

        public SegmentDisplayPriority(CGMFile container, int name, int prio)
            : this(container)
        {
            Name = name;
            Prio = prio;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Name = reader.ReadName();
            Prio = reader.ReadInt();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteName(Name);
            writer.WriteInt(Prio);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" SEGDISPPRI {WriteName(Name)} {WriteInt(Prio)};");
        }
    }
}
