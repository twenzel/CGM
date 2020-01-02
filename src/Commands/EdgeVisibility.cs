namespace codessentials.CGM.Commands
{
    public class EdgeVisibility : Command
    {
        public bool IsVisible { get; set; }

        public EdgeVisibility(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 30, container))
        {

        }

        public EdgeVisibility(CgmFile container, bool isVisible)
            : this(container)
        {
            IsVisible = isVisible;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            IsVisible = reader.ReadBool();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteBool(IsVisible);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  edgevis {WriteBool(IsVisible)};");
        }
    }
}