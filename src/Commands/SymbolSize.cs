namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=50
    /// </summary>
    public class SymbolSize : Command
    {
        public enum ScaleIndicator
        {
            HEIGHT, WIDTH, BOTH
        }

        public ScaleIndicator Indicator { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        public SymbolSize(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 50, container))
        {

        }

        public SymbolSize(CGMFile container, ScaleIndicator indicator, double width, double height)
            : this(container)
        {
            Indicator = indicator;
            Width = width;
            Height = height;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Indicator = (ScaleIndicator)reader.ReadEnum();
            Height = reader.ReadVdc();
            Width = reader.ReadVdc();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Indicator);
            writer.WriteVdc(Height);
            writer.WriteVdc(Width);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" SYMBOLSIZE {WriteEnum(Indicator)} {WriteVDC(Height)} {WriteVDC(Width)};");
        }
    }
}
