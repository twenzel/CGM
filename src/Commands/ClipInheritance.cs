namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=8, ElementId=3
    /// </summary>
    public class ClipInheritance : Command
    {
        public enum Value
        {
            STLIST = 0,
            INTERSECTION
        }

        public Value Data { get; private set; }

        public ClipInheritance(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.SegmentControlandSegmentAttributeElements, 3, container))
        {

        }

        public ClipInheritance(CgmFile container, Value value)
            : this(container)
        {
            Data = value;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Data = (Value)reader.ReadEnum();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Data);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" CLIPINH {WriteEnum(Data)};");
        }
    }
}