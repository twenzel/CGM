using System.Drawing;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=11
    /// </summary>
    public class RectangleElement : Command
    {
        public RectangleF Shape { get; private set; }
        public CGMPoint FirstCorner { get; set; }
        public CGMPoint SecondCorner { get; set; }

        public RectangleElement(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 11, container))
        {

        }

        public RectangleElement(CGMFile container, CGMPoint firstCorner, CGMPoint secondCorner)
            : this(container)
        {
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
            SetShape();
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            FirstCorner = reader.ReadPoint();
            SecondCorner = reader.ReadPoint();

            SetShape();
        }

        private void SetShape()
        {
            var x1 = (float)FirstCorner.X;
            var y1 = (float)FirstCorner.Y;
            var x2 = (float)SecondCorner.X;
            var y2 = (float)SecondCorner.Y;

            if (x1 > x2)
            {
                var temp = x1;
                x1 = x2;
                x2 = temp;
            }

            if (y1 > y2)
            {
                var temp = y1;
                y1 = y2;
                y2 = temp;
            }

            var w = x2 - x1;
            var h = y2 - y1;

            Shape = new RectangleF(x1, y1, w, h);
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(FirstCorner);
            writer.WritePoint(SecondCorner);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" RECT {WritePoint(FirstCorner)} {WritePoint(SecondCorner)};");
        }

        public override string ToString()
        {
            return $"RectangleElement [{FirstCorner.X},{FirstCorner.Y}] [{SecondCorner.X},{SecondCorner.Y}]";
        }
    }
}
