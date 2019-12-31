using codessentials.CGM.Classes;
using System.Collections.Generic;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=25
    /// </summary>
    public class NonUniformRationalBSpline : NonUniformBSpline
    {        
        public List<double> Weights { get; set; } = new List<double>();

        public NonUniformRationalBSpline(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 25, container))
        {           
           
        }

        public NonUniformRationalBSpline(CGMFile container, int splineOrder, IEnumerable<CGMPoint> points, IEnumerable<double> knots, double start, double end, IEnumerable<double> weights)
            : this(container)
        {
            Weights.AddRange(weights);
            SetValues(splineOrder, points, knots, start, end);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            base.ReadFromBinary(reader);

            for (int i = 0; i < Points.Count; i++)
                Weights.Add(reader.ReadReal());            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            base.WriteAsBinary(writer);
            foreach (var val in Weights)
                writer.WriteReal(val);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" NUB {WriteInt(SplineOrder)} {WriteInt(Points.Count)}");

            foreach (var val in Points)
                writer.Write($" {WritePoint(val)}");

            foreach (var val in Knots)
                writer.Write($" {WriteReal(val)}");

            writer.Write($" {WriteReal(StartValue)} {WriteReal(EndValue)}");

            foreach (var val in Weights)
                writer.Write($" {WriteReal(val)}");

            writer.WriteLine(";");
        }
    }
}