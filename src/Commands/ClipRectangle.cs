using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.IO;
using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=5
    /// </summary>
    public class ClipRectangle : Command
    {
        private System.Drawing.RectangleF _shape;
        public CGMPoint Point1 { get; set; }
        public CGMPoint Point2 { get; set; }

        public ClipRectangle(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 5, container))
        {
           
        }

        public ClipRectangle(CGMFile container,CGMPoint point1, CGMPoint point2)
            :this(container)
        {
            Point1 = point1;
            Point2 = point2;

            CreateShape();
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Point1 = reader.ReadPoint();
            Point2 = reader.ReadPoint();

            CreateShape();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(Point1);
            writer.WritePoint(Point2);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" CLIPRECT {WritePoint(Point1)} {WritePoint(Point2)};");
        }

        private void CreateShape()
        {
            _shape = new System.Drawing.RectangleF((float)Point1.X, (float)Point1.Y, (float)(Point2.X - Point1.X), (float)(Point2.Y - Point1.Y));
        }

        public override string ToString()
        {
            return $"ClipRectangle [{Point1.X},{Point1.Y}] [{Point2.X},{Point2.Y}]";
        }

    }
}