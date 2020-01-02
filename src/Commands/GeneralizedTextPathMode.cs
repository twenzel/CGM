namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=18
    /// </summary>
    public class GeneralizedTextPathMode : Command
    {
        public enum TextPathMode
        {
            OFF = 0,
            NONAXIS,
            AXIS
        }

        public TextPathMode Mode { get; set; }

        public GeneralizedTextPathMode(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 18, container))
        {
        }

        public GeneralizedTextPathMode(CgmFile container, TextPathMode mode)
            : this(container)
        {
            Mode = mode;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Mode = (TextPathMode)reader.ReadEnum();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Mode);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  GENTEXTPATHMODE {WriteEnum(Mode)};");
        }
    }
}