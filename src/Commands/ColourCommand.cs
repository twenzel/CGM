using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    public abstract class ColourCommand : Command
    {
        public CgmColor Color { get; set; }

        protected ColourCommand(CommandConstructorArguments args)
            : base(args)
        {

        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Color = reader.ReadColor();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteColor(Color);
        }

        protected void SetValue(CgmColor color)
        {
            Color = color;
        }
    }
}
