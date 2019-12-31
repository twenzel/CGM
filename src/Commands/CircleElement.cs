using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.IO;
using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=12
    /// </summary>
    public class CircleElement : Command
    {
        private CGMPoint _center;
        private double _radius;

        public CircleElement(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 12, container))
        {
           
        }

        public CircleElement(CGMFile container, CGMPoint center, double radius)
            :this(container)
        {
            _center = center;
            _radius = radius;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _center = reader.ReadPoint();
            _radius = reader.ReadVdc();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(_center);
            writer.WriteVdc(_radius);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  CIRCLE {WritePoint(_center)} {WriteVDC(_radius)};");
        }

        public override string ToString()
        {
            return $"Circle [{_center.X},{_center.Y}] {_radius}";
        }

        public CGMPoint Center => _center;
        public double Radius => _radius;
    }
}