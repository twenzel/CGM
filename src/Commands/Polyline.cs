using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.IO;
using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=1
    /// </summary>
    public class Polyline : Command, IComparable<Polyline>
    {
        private CGMPoint[] _points;

        public Polyline(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 1, container))
        {
            
        }

        public Polyline(CGMFile container, CGMPoint[] points)
            :this(container)
        {
            _points = points;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            int n = reader.Arguments.Length / reader.SizeOfPoint();

            _points = new CGMPoint[n];

            for (int i = 0; i < n; i++)
            {
                _points[i] = reader.ReadPoint();
            }            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach(var p in _points)
                writer.WritePoint(p);            
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write("  LINE");

            foreach(var points in _points)
                writer.Write($" {WritePoint(points)}");

            writer.WriteLine(";");
        }

        public override string ToString()
        {
            return "Polyline " + string.Join<CGMPoint>(", ", _points);
        }

        public bool IsSimpleLine
        {
            get { return _points.Length == 2; }
        }

        public CGMPoint[] Points
        {
            get { return _points; }
        }      

        public int CompareTo(Polyline other)
        {
            if (_points.Length == other._points.Length)
            {
                for (int i = 0; i < _points.Length; i++)
                {
                    var compareResult = _points[i].CompareTo(other._points[i]);
                    if (compareResult != 0)
                        return compareResult;
                }

                return 0;
            }
            else
                return _points.Length.CompareTo(other._points.Length);
        }
    }
}