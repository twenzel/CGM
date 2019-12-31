using System;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=23
    /// </summary>
    public class ParabolicArc : Command
    {
        public CGMPoint IntersectionPoint { get; set; }
        public CGMPoint Start { get; set; }
        public CGMPoint End { get; set; }

        public ParabolicArc(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 23, container))
        {
            
        }

        public ParabolicArc(CGMFile container, CGMPoint intersectionPoint, CGMPoint start, CGMPoint end)
            :this(container)
        {
            IntersectionPoint = intersectionPoint;
            Start = start;
            End = end;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            IntersectionPoint = reader.ReadPoint();
            Start = reader.ReadPoint();
            End = reader.ReadPoint();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(IntersectionPoint);
            writer.WritePoint(Start);
            writer.WritePoint(End);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" PARABARC {WritePoint(IntersectionPoint)} {WritePoint(Start)} {WritePoint(End)};");
        }
    }
}