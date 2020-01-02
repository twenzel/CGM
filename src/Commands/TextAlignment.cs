namespace codessentials.CGM.Commands
{
    public class TextAlignment : Command
    {
        public enum HorizontalAlignmentType { NORMHORIZ, LEFT, CTR, RIGHT, CONTHORIZ }
        public enum VerticalAlignmentType { NORMVERT, TOP, CAP, HALF, BASE, BOTTOM, CONTVERT }

        public HorizontalAlignmentType HorizontalAlignment { get; set; }
        public VerticalAlignmentType VerticalAlignment { get; set; }
        public double ContinuousHorizontalAlignment { get; set; }
        public double ContinuousVerticalAlignment { get; set; }

        public TextAlignment(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 18, container))
        {

        }

        public TextAlignment(CGMFile container, HorizontalAlignmentType horz, VerticalAlignmentType vert, double continousHorz, double continousVert)
            : this(container)
        {
            HorizontalAlignment = horz;
            VerticalAlignment = vert;
            ContinuousHorizontalAlignment = continousHorz;
            ContinuousVerticalAlignment = continousVert;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var enumValueHorz = reader.ReadEnum();
            switch (enumValueHorz)
            {
                case 0:
                    HorizontalAlignment = HorizontalAlignmentType.NORMHORIZ;
                    break;
                case 1:
                    HorizontalAlignment = HorizontalAlignmentType.LEFT;
                    break;
                case 2:
                    HorizontalAlignment = HorizontalAlignmentType.CTR;
                    break;
                case 3:
                    HorizontalAlignment = HorizontalAlignmentType.RIGHT;
                    break;
                case 4:
                    HorizontalAlignment = HorizontalAlignmentType.CONTHORIZ;
                    break;
                default:
                    HorizontalAlignment = HorizontalAlignmentType.NORMHORIZ;
                    reader.Unsupported("unsupported horizontal alignment " + enumValueHorz);
                    break;
            }

            var enumValueVert = reader.ReadEnum();
            switch (enumValueVert)
            {
                case 0:
                    VerticalAlignment = VerticalAlignmentType.NORMVERT;
                    break;
                case 1:
                    VerticalAlignment = VerticalAlignmentType.TOP;
                    break;
                case 2:
                    VerticalAlignment = VerticalAlignmentType.CAP;
                    break;
                case 3:
                    VerticalAlignment = VerticalAlignmentType.HALF;
                    break;
                case 4:
                    VerticalAlignment = VerticalAlignmentType.BASE;
                    break;
                case 5:
                    VerticalAlignment = VerticalAlignmentType.BOTTOM;
                    break;
                case 6:
                    VerticalAlignment = VerticalAlignmentType.CONTVERT;
                    break;
                default:
                    VerticalAlignment = VerticalAlignmentType.NORMVERT;
                    reader.Unsupported("unsupported vertical alignment " + enumValueVert);
                    break;
            }

            ContinuousHorizontalAlignment = reader.ReadReal();
            ContinuousVerticalAlignment = reader.ReadReal();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)HorizontalAlignment);
            writer.WriteEnum((int)VerticalAlignment);
            writer.WriteReal(ContinuousHorizontalAlignment);
            writer.WriteReal(ContinuousVerticalAlignment);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  textalign {WriteEnum(HorizontalAlignment)}, {WriteEnum(VerticalAlignment)}, {WriteDouble(ContinuousHorizontalAlignment)}, {WriteDouble(ContinuousVerticalAlignment)};");
        }

    }
}
