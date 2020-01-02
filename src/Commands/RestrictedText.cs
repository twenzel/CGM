using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=5
    /// </summary>
    public class RestrictedText : TextCommand
    {
        public double DeltaWidth { get; set; }
        public double DeltaHeight { get; set; }
        public bool Final { get; set; }

        public RestrictedText(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 5, container))
        {

        }

        public RestrictedText(CGMFile container, string data, CGMPoint position, double deltaWidth, double deltaHeight, bool final)
            : this(container)
        {
            DeltaWidth = deltaWidth;
            DeltaHeight = deltaHeight;
            Final = final;
            SetValues(data, position);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            DeltaWidth = reader.ReadVdc();
            DeltaHeight = reader.ReadVdc();
            Position = reader.ReadPoint();

            Final = reader.ReadBool();

            Text = reader.ReadString();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteVdc(DeltaWidth);
            writer.WriteVdc(DeltaHeight);
            writer.WritePoint(Position);
            writer.WriteBool(Final);
            writer.WriteString(Text);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($"  RESTRTEXT");
            writer.Write(" " + WriteDouble(DeltaWidth));
            writer.Write(" " + WriteDouble(DeltaHeight));
            writer.Write("  " + WritePoint(Position));

            if (Final)
                writer.Write($" final");
            else
                writer.Write($" notfinal");

            writer.Write($" {WriteString(Text)}");

            writer.WriteLine(";");
        }

        public override string ToString()
        {
            return $"RestrictedText {Text} deltaWidth={DeltaWidth} deltaHeight={DeltaHeight} textPosition.x={Position.X} textPosition.y={Position.Y}";
        }
    }
}
