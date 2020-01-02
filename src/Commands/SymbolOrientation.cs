namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=51
    /// </summary>
    public class SymbolOrientation : Command
    {
        public double UpX { get; set; }
        public double UpY { get; set; }
        public double BaseX { get; set; }
        public double BaseY { get; set; }

        public SymbolOrientation(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 51, container))
        {

        }

        public SymbolOrientation(CGMFile container, double upX, double upY, double baseX, double baseY)
            : this(container)
        {
            UpX = upX;
            UpY = upY;
            BaseX = baseX;
            BaseY = baseY;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            UpX = reader.ReadVdc();
            UpY = reader.ReadVdc();
            BaseX = reader.ReadVdc();
            BaseY = reader.ReadVdc();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteVdc(UpX);
            writer.WriteVdc(UpY);
            writer.WriteVdc(BaseX);
            writer.WriteVdc(BaseY);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" SYMBOLORI {WriteVDC(UpX)} {WriteVDC(UpY)} {WriteVDC(BaseX)} {WriteVDC(BaseY)};");
        }
    }
}
