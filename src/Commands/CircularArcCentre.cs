using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.IO;
using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=15
    /// </summary>
    public class CircularArcCentre : Command
    {
        protected CGMPoint _center;
        protected double _startDeltaX;
        protected double _startDeltaY;
        protected double _endDeltaX;
        protected double _endDeltaY;
        protected double _radius;

        public CircularArcCentre(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 15, container))
        {
                     
        }

        public CircularArcCentre(CGMFile container, CGMPoint center, double startDeltaX, double startDeltaY, double endDeltaX, double endDeltaY, double radius)
           : this(container)
        {
            SetValues(center, startDeltaX, startDeltaY, endDeltaX, endDeltaY, radius);
        }       

        public CircularArcCentre(CommandConstructorArguments args)
            :base(args)
        {
            
        }

        protected void SetValues(CGMPoint center, double startDeltaX, double startDeltaY, double endDeltaX, double endDeltaY, double radius)
        {
            _center = center;
            _startDeltaX = startDeltaX;
            _startDeltaY = startDeltaY;
            _endDeltaX = endDeltaX;
            _endDeltaY = endDeltaY;
            _radius = radius;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _center = reader.ReadPoint();
            _startDeltaX = reader.ReadVdc();
            _startDeltaY = reader.ReadVdc();
            _endDeltaX = reader.ReadVdc();
            _endDeltaY = reader.ReadVdc();
            _radius = reader.ReadVdc();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(_center);
            writer.WriteVdc(_startDeltaX);
            writer.WriteVdc(_startDeltaY);
            writer.WriteVdc(_endDeltaX);
            writer.WriteVdc(_endDeltaY);
            writer.WriteVdc(_radius);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write("  ARCCTR");
            WriteValues(writer);
            writer.WriteLine(" ;");
        }

        protected virtual void WriteValues(IClearTextWriter writer)
        {
            writer.Write($" {WritePoint(_center)}");
            writer.Write($" {WritePoint(_startDeltaX, _startDeltaY)} {WritePoint(_endDeltaX, _endDeltaY)}");
            writer.Write($" {WriteVDC(_radius)}");
        }

        public CGMPoint Center => _center;
        public double StartDeltaX => _startDeltaX;
        public double StartDeltaY => _startDeltaY;
        public double EndDeltaX => _endDeltaX;
        public double EndDeltaY => _endDeltaY;
        public double Radius => _radius;
    }
}