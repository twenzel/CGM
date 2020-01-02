namespace codessentials.CGM.Commands
{
    public abstract class CapCommand : Command
    {
        public LineCapIndicator LineIndicator { get; set; }
        public DashCapIndicator DashIndicator { get; set; }

        protected CapCommand(CommandConstructorArguments arguments)
            : base(arguments)
        {

        }


        public override void ReadFromBinary(IBinaryReader reader)
        {
            var lineIndic = reader.ReadIndex();
            switch (lineIndic)
            {
                case 1:
                    LineIndicator = LineCapIndicator.UNSPECIFIED;
                    break;
                case 2:
                    LineIndicator = LineCapIndicator.BUTT;
                    break;
                case 3:
                    LineIndicator = LineCapIndicator.ROUND;
                    break;
                case 4:
                    LineIndicator = LineCapIndicator.PROJECTING_SQUARE;
                    break;
                case 5:
                    LineIndicator = LineCapIndicator.TRIANGLE;
                    break;
                default:
                    reader.Unsupported("unsupported line cap indicator " + lineIndic);
                    LineIndicator = LineCapIndicator.UNSPECIFIED;
                    break;
            }

            var dashIndic = reader.ReadIndex();
            switch (dashIndic)
            {
                case 1:
                    DashIndicator = DashCapIndicator.UNSPECIFIED;
                    break;
                case 2:
                    DashIndicator = DashCapIndicator.BUTT;
                    break;
                case 3:
                    DashIndicator = DashCapIndicator.MATCH;
                    break;
                default:
                    reader.Unsupported("unsupported dash cap indicator " + dashIndic);
                    DashIndicator = DashCapIndicator.UNSPECIFIED;
                    break;
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex((int)LineIndicator);
            writer.WriteIndex((int)DashIndicator);
        }

        protected void SetValues(LineCapIndicator lineIndicator, DashCapIndicator dashIndicator)
        {
            LineIndicator = lineIndicator;
            DashIndicator = dashIndicator;
        }
    }
}
