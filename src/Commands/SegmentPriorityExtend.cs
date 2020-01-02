namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=18
    /// </remarks>
    public class SegmentPriorityExtend : Command
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public SegmentPriorityExtend(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 18, container))
        {

        }

        public SegmentPriorityExtend(CGMFile container, int min, int max)
            : this(container)
        {
            Min = min;
            Max = max;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Min = reader.ReadInt();
            Max = reader.ReadInt();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(Min);
            writer.WriteInt(Max);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" SEGPRIEXT {WriteInt(Min)} {WriteInt(Max)};");
        }
    }
}
