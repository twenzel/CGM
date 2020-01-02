namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=6
    /// </summary>
    public class ClipIndicator : Command
    {
        public bool Flag { get; private set; }

        public ClipIndicator(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 6, container))
        {

        }

        public ClipIndicator(CGMFile container, bool flag)
            : this(container)
        {
            Flag = flag;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Flag = reader.ReadBool();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteBool(Flag);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  clip {WriteBool(Flag)};");
        }

        public override string ToString()
        {
            return $"ClipIndicator {Flag}";
        }        
    }
}