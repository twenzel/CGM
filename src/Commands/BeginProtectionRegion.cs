namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=13
    /// </remarks>
    public class BeginProtectionRegion : Command
    {
        public int RegionIndex { get; private set; }

        public BeginProtectionRegion(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 13, container))
        {

        }

        public BeginProtectionRegion(CgmFile container, int index)
            : this(container)
        {
            RegionIndex = index;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            RegionIndex = reader.ReadIndex();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(RegionIndex);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" BEGPROTREGION {WriteIndex(RegionIndex)};");
        }
    }
}