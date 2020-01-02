﻿using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=5
    /// </summary>
    public class ClipRectangle : Command
    {
        public CGMPoint Point1 { get; set; }
        public CGMPoint Point2 { get; set; }

        public ClipRectangle(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 5, container))
        {

        }

        public ClipRectangle(CGMFile container, CGMPoint point1, CGMPoint point2)
            : this(container)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Point1 = reader.ReadPoint();
            Point2 = reader.ReadPoint();
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

        public override string ToString()
        {
            return $"ClipRectangle [{Point1.X},{Point1.Y}] [{Point2.X},{Point2.Y}]";
        }

    }
}